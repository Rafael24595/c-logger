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
                Console.WriteLine($"Field Name: {fieldAttribute.FieldName}, Field Type: {fieldAttribute.FieldType}");
            }
        }
        return attributes;
    }

}