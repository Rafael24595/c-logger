using System.Reflection;

public class Misc {

    public static List<FieldAttribute> LogEventStructure() {
        Type eventType = typeof(LogEvent);
        List<FieldAttribute> attributes = [];
        foreach (PropertyInfo property in eventType.GetProperties()) {
            var attribute = Attribute.GetCustomAttribute(property, typeof(FieldAttribute));
            if (attribute != null) {
                FieldAttribute fieldAttribute = (FieldAttribute) attribute;
                attributes.Add(fieldAttribute);
            }
        }
        return attributes;
    }

    public static List<(FieldAttribute, object)> LogEventStructure(LogEvent log) {
        Type eventType = typeof(LogEvent);
        List<(FieldAttribute, object)> attributes = [];
        foreach (PropertyInfo property in eventType.GetProperties()) {
            var attribute = Attribute.GetCustomAttribute(property, typeof(FieldAttribute));
            var value = property.GetValue(log);
            if (attribute != null && value != null) {
                FieldAttribute fieldAttribute = (FieldAttribute) attribute;
                attributes.Add((fieldAttribute, value));
            }
        }
        return attributes;
    }

    public static LogEvent LogEventFromJson(Dictionary<string, object> dict) {
        Type eventType = typeof(LogEvent);
        LogEvent log = new();
        foreach (PropertyInfo property in eventType.GetProperties()) {
            var attribute = Attribute.GetCustomAttribute(property, typeof(FieldAttribute));
            if (attribute != null) {
                FieldAttribute fieldAttribute = (FieldAttribute) attribute;
                var value = dict.GetValueOrDefault(fieldAttribute.FieldName);
                property.SetValue(log, value);
            }
        }
        return log;
    }

}