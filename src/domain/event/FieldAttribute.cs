[AttributeUsage(AttributeTargets.Property)]
public class FieldAttribute : Attribute {

    private static readonly int MAX_SIZE = 21844;
    public string FieldName { get; }
    public EFieldType FieldType { get; }
    public int FieldSize { get; }
    public EFieldKey KeyStatus { get; }

    public FieldAttribute(string fieldName, EFieldType fieldType) : this(fieldName, fieldType, MAX_SIZE) {
    }

    public FieldAttribute(string fieldName, EFieldKey keyStatus, EFieldType fieldType) : this(fieldName, keyStatus, fieldType, MAX_SIZE) {
    }

    public FieldAttribute(string fieldName, EFieldType fieldType, int fieldSize) : this(fieldName, EFieldKey.IS_NOT_KEY, fieldType, fieldSize) {
    }

    public FieldAttribute(string fieldName, EFieldKey keyStatus, EFieldType fieldType, int fieldSize) {
        this.KeyStatus = keyStatus;
        this.FieldName = fieldName;
        this.FieldType = fieldType;
        this.FieldSize = fieldSize;
    }

}