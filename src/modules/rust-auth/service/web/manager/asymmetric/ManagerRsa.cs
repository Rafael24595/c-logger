using System.Security.Cryptography;
using System.Text;

class ManagerRsa: IManagerAsymmetric {

    public const string NAME = "RSA";

    public ManagerRsa() {
    }

    public string Encrypt(PubKey pubkey, string message) {
        RSACryptoServiceProvider rsa = new();
        rsa.ImportFromPem(pubkey.key);

        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        byte[] encryptedBytes = rsa.Encrypt(messageBytes, false);

        return Convert.ToBase64String(encryptedBytes);
    }

}