using System.Security.Cryptography;
using System.Text;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Options;
using Microsoft.Extensions.Options;

namespace Learnify.Core.Managers;

/// <inheritdoc />
public class EncryptionHelper: IEncryptionHelper
{
    private static EncryptionOptions _encryption;

    /// <summary>
    /// Initializes new instance <see cref="EncryptionHelper"/>
    /// </summary>
    /// <param name="encryptionOptions"></param>
    public EncryptionHelper(IOptions<EncryptionOptions> encryptionOptions)
    {
        _encryption = encryptionOptions.Value;
    }

    /// <inheritdoc />
    public string Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryption.Key);
        aes.IV = Encoding.UTF8.GetBytes(_encryption.IV);

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
            }

            return Convert.ToBase64String(ms.ToArray());
        }
    }

    /// <inheritdoc />
    public string Decrypt(string cipherText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryption.Key);
        aes.IV = Encoding.UTF8.GetBytes(_encryption.IV);

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText));
        using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}