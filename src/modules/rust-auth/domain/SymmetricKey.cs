public class SymmetricKey {

    public string module { get; set; }
    public string key { get; set; }
    public string format { get; set; }
    public long expires { get; set; }
    
    public SymmetricKey(string module, string key, string format, long expires) {
        this.module = module;
        this.key = key;
        this.format = format;
        this.expires = expires;
    }

}