interface IManagerSymmetric {
    public string Key();
    Optional<string> Status();
    public string Encrypt(string message);
    public string Decrypt(string message);
}