using System.Reflection;

public class SQLQuery {

    public static string SelectSchemaName(string dataBase) {
        return $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{dataBase}'";
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

    private static string FieldToSQL(FieldAttribute a) {
        List<string> query = [a.FieldName];

        if(a.FieldType == EFieldType.INTEGER) {
            query.Add("INT");
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