using System.Reflection;

public class SQLQuery {

    public static string SelectSchemaName(string dataBase) {
        return $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{dataBase}'";
    }

    public static string Select(string table, string id) {
        return $"SELECT * FROM {table} WHERE id = {id}";
    }

    public static string SelectWhere(string table, string service, string session_id) {
        return $"SELECT * FROM {table} WHERE service = '{service}' AND session_id = '{session_id}'";
    }

    public static string ShowTablesLike(string table) {
        return $"SHOW TABLES LIKE '{table}'";
    }

    public static string CreateDataBase(string dataBase) {
        return $"CREATE DATABASE {dataBase}";
    }

    public static string CreateLogEventTable(string table) {
        List<FieldAttribute> attributes = Misc.LogEventStructure();
        List<string> query = [$"CREATE TABLE {table} ("];
        List<string> fields = [];
        List<string> keys = [];
        
        foreach (var attribute in attributes) {
            if(attribute.KeyStatus.IsKey()) {
                keys.Add(attribute.FieldName);
            }
            fields.Add(FieldToSQL(attribute));
        }
        
        if(keys.Count > 0) {
            fields.Add("PRIMARY KEY(" + string.Join(",", keys) + ")");
        }

        query.Add(string.Join(",", fields));
        query.Add(")");

        return string.Join("", query);
    }

    public static string InsertLogEvent(string table, LogEvent log) {
        List<(FieldAttribute, object)> attributes = Misc.LogEventStructure(log);
        List<string> query = [$"INSERT INTO {table}"];
        List<string> fields = [];
        List<string> values = [];
        
        foreach (var attribute in attributes) {
            if(attribute.Item1.KeyStatus.IsNotKey()) {
                var value = attribute.Item2.ToString();
                if(attribute.Item1.FieldType == EFieldType.STRING) {
                    value = $"\"{value}\"";
                }
                fields.Add(attribute.Item1.FieldName);
                values.Add(value ?? "");
            }
        }

        query.Add($"({string.Join(", ", fields)})");
        query.Add("VALUES");
        query.Add($"({string.Join(", ", values)})");

        return string.Join(" ", query);
    }

    private static string FieldToSQL(FieldAttribute a) {
        List<string> query = [a.FieldName];

        if(a.FieldType == EFieldType.INTEGER) {
            query.Add("INT");
        }

         if(a.FieldType == EFieldType.BIGINT) {
            query.Add("BIGINT");
        }

        if(a.KeyStatus.IsKey() && a.FieldType == EFieldType.INTEGER) {
            query.Add("AUTO_INCREMENT");
        }

        if(a.FieldType == EFieldType.STRING) {
            string type = "VARCHAR";
            if(a.FieldSize > 255) {
                type = "TEXT";
            }
            query.Add(type + "(" + a.FieldSize + ")");
        }

        return string.Join(" ", query);
    }

    public static string DescribeTable(string table) {
        return $"DESCRIBE {table}";
    }

}