using MySql.Data.MySqlClient;

public class RepositoryMySQL: IRepository {

    public const string NAME = "RepositoryMySQL";

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
    
    private Result<MySqlConnection, LogApiException> GetConnection() {
        MySqlConnection connection = new(this.connection);
        
        try {
            connection.Open();

            if (!DatabaseExists(connection)) {
                CreateDatabase(connection);
            }

            connection.ChangeDatabase(this.database);

            if (!TableExists(connection)) {
                CreateTable(connection);
            }

            if(!TableHasExpectedStructure(connection)) {
                LogApiException exception = new LogApiException(500, "", "Table description changed!");
                return Result<MySqlConnection, LogApiException>.ERR(exception);
            }
        } catch (Exception e) {
            LogApiException exception = new LogApiException(500, "", e);
            return Result<MySqlConnection, LogApiException>.ERR(exception);
        }

        return Result<MySqlConnection, LogApiException>.OK(connection);
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

    public Result<LogEvent, LogApiException> Find(string id) {
        var oConnection = this.GetConnection();
        if(oConnection.IsErr()) {
            Optional<LogApiException> err = oConnection.Err();
            return Result<LogEvent, LogApiException>.ERR(err.Unwrap());
        }

        using var connection = oConnection.Ok().Unwrap();

        try {
            using MySqlCommand command = new(SQLQuery.Select(this.collection, id), connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                Dictionary<string, object> row = this.LogEventReaderAsJson(reader);
                return Result<LogEvent, LogApiException>.OK(Misc.LogEventFromJson(row));
            }
        } catch (Exception e) {
            LogApiException exct = new LogApiException(500, "", e);
            return Result<LogEvent, LogApiException>.ERR(exct);
        }

        LogApiException exception = new LogApiException(404, "", "Logs not found.");
        return Result<LogEvent, LogApiException>.ERR(exception);
    }

    public Result<List<LogEvent>, LogApiException> FindAll(string service, string session_id) {
        var oConnection = this.GetConnection();
        if(oConnection.IsErr()) {
            Optional<LogApiException> err = oConnection.Err();
            return Result<List<LogEvent>, LogApiException>.ERR(err.Unwrap());
        }

        using var connection = oConnection.Ok().Unwrap();

        try {
            using MySqlCommand command = new(SQLQuery.SelectWhere(this.collection, service, session_id), connection);
            using MySqlDataReader reader = command.ExecuteReader();

            List<LogEvent> logs = [];

            while (reader.Read()) {
                Dictionary<string, object> row = this.LogEventReaderAsJson(reader);
                var log = Misc.LogEventFromJson(row);
                logs.Add(log);
            }

             return Result<List<LogEvent>, LogApiException>.OK(logs);
        }
        catch (Exception e) {
            LogApiException exct = new LogApiException(500, "", e);
            return Result<List<LogEvent>, LogApiException>.ERR(exct);
        }
    }

    public Result<LogEvent, LogApiException> Insert(LogEvent log) {
        var oConnection = this.GetConnection();
        if(oConnection.IsErr()) {
            Optional<LogApiException> err = oConnection.Err();
            return Result<LogEvent, LogApiException>.ERR(err.Unwrap());
        }

        using var connection = oConnection.Ok().Unwrap();

        try {
            using MySqlCommand command = new(SQLQuery.InsertLogEvent(this.collection, log), connection);
            command.ExecuteNonQuery();

            return Result<LogEvent, LogApiException>.OK(log);
        } catch (Exception e) {
            LogApiException exception = new LogApiException(500, "", e);
            return Result<LogEvent, LogApiException>.ERR(exception);
        }
    }

    public bool Reset() {
        var oConnection = this.GetConnection();
        if(oConnection.IsErr()) {
            return false;
        }

        using var connection = oConnection.Ok().Unwrap();

        try {
        using MySqlCommand drop = new($"DROP TABLE {this.collection}", connection);
        drop.ExecuteNonQuery();

        return true;
        }
        catch (Exception e) {
            return false;
        }
    }

     public Dictionary<string, object> LogEventReaderAsJson(MySqlDataReader reader) {
        Dictionary<string, object> row = [];
        for (int i = 0; i < reader.FieldCount; i++) {
            string columnName = reader.GetName(i);
            object value = reader.GetValue(i);
            row[columnName] = value;
        }
        return row;
    }

}