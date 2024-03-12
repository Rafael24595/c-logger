using System.Security.Cryptography;
using System.Text;

class ManagerAesGcm: IManagerSymmetric {

    public const string NAME = "AES_GCM";

    private readonly int size;
    private readonly byte[] tag;
    private readonly byte[] key;

    public ManagerAesGcm(int size) {
        this.size = size;
        this.tag = new byte[16];
        this.key = this.RandomKey(this.KeySizeFromSize(size));
    }

    public SymmetricKey Key() {
        var key = this.ByteArrayToHexString(this.key);
        var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (15 * 60 * 1000);
        return new SymmetricKey(NAME, key, this.size.ToString(), timestamp);
    }

    public Optional<string> Status() {
        try {
            var input = "Hello status!";
            var enc = this.Encrypt(input);
            var dec = this.Decrypt(enc);
            if(!input.Equals(dec)) {
                return Optional<string>.Some("Input does not matched with decrypted result.");
            }
            return Optional<string>.None();
        } catch (Exception e) {
            return Optional<string>.Some(e.Message);
        }
    }

    public string Encrypt(string message) {
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        using AesGcm aesGcm = new(this.key, this.tag.Length);
        byte[] nonce = new byte[12];
        byte[] ciphertext = new byte[bytes.Length];

        aesGcm.Encrypt(nonce, bytes, ciphertext, tag);

        return Convert.ToBase64String(ciphertext);
    }

    public string Decrypt(string message) {
        byte[] bytes = Convert.FromBase64String(message);
        using AesGcm aesGcm = new(this.key, this.tag.Length);
        byte[] nonce = new byte[12];
        byte[] decryptedText = new byte[bytes.Length];

        aesGcm.Decrypt(nonce, bytes, tag, decryptedText);

        return Encoding.UTF8.GetString(decryptedText);
    }

    private int KeySizeFromSize(int size) {
        return size switch {
            256 => 32,
            _ => size,
        };
    }

    private byte[] RandomKey(int lengthInBytes) {
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] randomBytes = new byte[lengthInBytes];
        rng.GetBytes(randomBytes);
        return randomBytes;
    }

    private string ByteArrayToHexString(byte[] array) {
        StringBuilder hex = new(array.Length * 2);
        foreach (byte b in array)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }

}