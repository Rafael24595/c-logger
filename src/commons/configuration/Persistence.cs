public class Persistence {

    public readonly string code;
    public readonly string connection;
    public readonly string database;
    public readonly string collection;

    internal Persistence(string code, string connection, string database, string collection) {
        this.code = code;
        this.connection = connection;
        this.database = database;
        this.collection = collection;
    }
    
}