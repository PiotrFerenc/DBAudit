using System.Security.Cryptography;
using System.Text;

namespace DBAudit.Infrastructure;

public class EncryptionService(string key, string iv) : IEncryptionService
{
    private readonly byte[] _key = Convert.FromBase64String(key);
    private readonly byte[] _iv = Convert.FromBase64String(iv);

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);

        var encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string encryptedText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        var decryptor = aes.CreateDecryptor();
        var encryptedBytes = Convert.FromBase64String(encryptedText);

        var decrypted = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        return Encoding.UTF8.GetString(decrypted);
    }
}