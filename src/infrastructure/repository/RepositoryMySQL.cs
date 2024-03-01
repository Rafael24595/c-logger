using MySql.Data.MySqlClient;

public class RepositoryMySQL: IRepository {

    private readonly string connection;
    private readonly string database;
    private readonly string collection;

    public RepositoryMySQL(Persistence persistence): this(persistence.connection, persistence.database, persistence.collection) {
    }

    public RepositoryMySQL(string connection, string database, string collection) {
        this.connection = connection;
        this.database = database;
        this.collection = collection;
    }
    
    private Optional<MySqlConnection> GetConnection() {
        MySqlConnection connection = new(this.connection);
        
        try {
            connection.Open();
            Console.WriteLine("Connection opened successfully!");

            if (!DatabaseExists(connection)) {
                CreateDatabase(connection);
                Console.WriteLine("Database created successfully!");
            }

            connection.ChangeDatabase(this.database);

            if (!TableExists(connection)) {
                CreateTable(connection);
                Console.WriteLine("Table created successfully!");
            }

            if(!TableHasExpectedStructure(connection)) {
                Console.WriteLine("Table description changed!");
                return Optional<MySqlConnection>.None();
            }
        } catch (Exception e) {
            Console.WriteLine($"Error: {e.Message}");
            return Optional<MySqlConnection>.None();
        }

        return Optional<MySqlConnection>.Some(connection);
    }

    private bool DatabaseExists(MySqlConnection connection) {
        using MySqlCommand command = new(SQLQuery.SelectSchemaName(this.database), connection);
        return command.ExecuteScalar() != null;
    }

    private void CreateDatabase(MySqlConnection connection) {
        using MySqlCommand command = new(SQLQuery.CreateDataBase(this.database), connection);
        command.ExecuteNonQuery();
    }

    private bool TableExists(MySqlConnection connection) {
        using MySqlCommand command = new(SQLQuery.ShowTablesLike(this.collection), connection);
        return command.ExecuteScalar() != null;
    }

    private void CreateTable(MySqlConnection connection) {
        string query = SQLQuery.CreateLogEventTable(this.collection);
        using MySqlCommand command = new(query, connection);
        command.ExecuteNonQuery();
    }

    private bool TableHasExpectedStructure(MySqlConnection connection) {
        try {
            using MySqlCommand command = new(SQLQuery.DescribeTable(this.collection), connection);
            using MySqlDataReader reader = command.ExecuteReader();

            List<string> expectedColumns = Misc.LogEventStructure()
                .Select(a => a.FieldName)
                .ToList();

            while (reader.Read()) {
                var columnName = reader["Field"];
                if (columnName != null && !expectedColumns.Contains(columnName?.ToString())) {
                    return false;
                }
            }

            return expectedColumns.All(column => reader.HasRows);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return true;
        }
    }

    public Optional<LogEvent> Find(string id) {
        var oConnection = this.GetConnection();
        if(oConnection.IsNone()) {
            return Optional<LogEvent>.None();
        }

        using var connection = oConnection.Unwrap();

        try {
            using MySqlCommand command = new(SQLQuery.Select(this.collection, id), connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                Dictionary<string, object> row = this.LogEventAsJson(reader);
                return Optional<LogEvent>.Some(Misc.LogEventFromJson(row));
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }

        return Optional<LogEvent>.None();
    }

    public Optional<List<LogEvent>> FindAll(string service, string session_id) {
        var oConnection = this.GetConnection();
        if(oConnection.IsNone()) {
            return Optional<List<LogEvent>>.None();
        }

        using var connection = oConnection.Unwrap();

        try {
            using MySqlCommand command = new(SQLQuery.SelectWhere(this.collection, service, session_id), connection);
            using MySqlDataReader reader = command.ExecuteReader();

            List<LogEvent> logs = [];

            while (reader.Read()) {
                Dictionary<string, object> row = this.LogEventAsJson(reader);
                var log = Misc.LogEventFromJson(row);
                logs.Add(log);
            }

             return Optional<List<LogEvent>>.Some(logs);
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }

        return Optional<List<LogEvent>>.None();
    }

    public Optional<LogEvent> Insert(LogEvent log) {
        var oConnection = this.GetConnection();
        if(oConnection.IsNone()) {
            return Optional<LogEvent>.None();
        }

        using var connection = oConnection.Unwrap();

        using MySqlCommand command = new(SQLQuery.InsertLogEvent(this.collection, log), connection);
        command.ExecuteNonQuery();

        return Optional<LogEvent>.Some(log);
    }

    public bool Reset(LogEvent log) {
        var oConnection = this.GetConnection();
        if(oConnection.IsNone()) {
            return false;
        }

        using var connection = oConnection.Unwrap();

        using MySqlCommand drop = new($"DROP TABLE {this.collection}", connection);
        drop.ExecuteNonQuery();

        return true;
    }

     public Dictionary<string, object> LogEventAsJson(MySqlDataReader reader) {
        Dictionary<string, object> row = [];
        for (int i = 0; i < reader.FieldCount; i++) {
            string columnName = reader.GetName(i);
            object value = reader.GetValue(i);

            row[columnName] = value;
        }
        return row;
    }

}