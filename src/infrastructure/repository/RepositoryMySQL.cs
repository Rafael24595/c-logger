using MySql.Data.MySqlClient;

public class RepositoryMySQL {

    public static void Test() {
        string connectionString = "Server=localhost;Port=3306;User ID=root;Password=example;";
        string dataBase = "logger";
        string table = "logs";

        using MySqlConnection connection = new(connectionString);
        
        try {
            connection.Open();
            Console.WriteLine("Connection opened successfully!");

            if (!DatabaseExists(connection, dataBase)) {
                CreateDatabase(connection, dataBase);
                Console.WriteLine("Database created successfully!");
            }

            connection.ChangeDatabase(dataBase);

            if (!TableExists(connection, table)) {
                CreateTable(connection, table);
                Console.WriteLine("Table created successfully!");
            }

            if(!TableHasExpectedStructure(connection, table)) {
                Console.WriteLine("Table description changed!");
            }
        }
        catch (Exception e) {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    private static bool DatabaseExists(MySqlConnection connection, string dataBase) {
        using MySqlCommand command = new(SQLQuery.SelectSchemaName(dataBase), connection);
        return command.ExecuteScalar() != null;
    }

    private static void CreateDatabase(MySqlConnection connection, string dataBase) {
        using MySqlCommand command = new(SQLQuery.CreateDataBase(dataBase), connection);
        command.ExecuteNonQuery();
    }

    private static bool TableExists(MySqlConnection connection, string table) {
        using MySqlCommand command = new(SQLQuery.ShowTablesLike(table), connection);
        return command.ExecuteScalar() != null;
    }

    private static void CreateTable(MySqlConnection connection, string tableName) {
        string query = SQLQuery.CreateLogEventTable(tableName);
        using MySqlCommand command = new(query, connection);
        command.ExecuteNonQuery();
    }

    private static bool TableHasExpectedStructure(MySqlConnection connection, string table) {
        try {
            using MySqlCommand command = new(SQLQuery.DescribeTable(table), connection);
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

}