namespace Learnify.Core.ManagerContracts;

public interface IEncryptionHelper
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}