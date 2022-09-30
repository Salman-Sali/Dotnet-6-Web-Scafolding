using Shared.Configrations;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Shared
{
    public interface IAESCipher
    {
        public string Encrypt(string stringToEncrypt);
        public string Decrypt(string stringToDecrypt);
    }

    public class AESCipher : IAESCipher
    {
        private readonly AppSettings _appSettings;
        public AESCipher(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        const string KEY_STATIC_PAIR = "%Ig1O47";
        const string INITIAL_VECTOR_STATIC_PAIR = "24p$C7c";
        public string Encrypt(string stringToEncrypt)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
            {
                return null;
            }

            var key = _appSettings.Cipher.CypherKey;
            var initialVector = _appSettings.Cipher.InitialVector;

            key += KEY_STATIC_PAIR;
            initialVector += INITIAL_VECTOR_STATIC_PAIR;

            var utf8 = new UTF8Encoding();
            byte[] byteArrayToEncrypt = utf8.GetBytes(stringToEncrypt);
            AesCryptoServiceProvider dataEncrypt = new AesCryptoServiceProvider();
            dataEncrypt.BlockSize = 128;
            dataEncrypt.KeySize = 128;
            dataEncrypt.Key = System.Text.Encoding.UTF8.GetBytes(key);
            dataEncrypt.IV = System.Text.Encoding.UTF8.GetBytes(initialVector);
            dataEncrypt.Padding = PaddingMode.PKCS7;
            dataEncrypt.Mode = CipherMode.CBC;
            ICryptoTransform crypto = dataEncrypt.CreateEncryptor(dataEncrypt.Key, dataEncrypt.IV);
            byte[] encryptedData = crypto.TransformFinalBlock(byteArrayToEncrypt, 0, byteArrayToEncrypt.Length);
            crypto.Dispose();
            return Convert.ToBase64String(encryptedData);
        }

        public string Decrypt(string stringToDecrypt)
        {
            if (string.IsNullOrEmpty(stringToDecrypt))
            {
                return null;
            }

            var key = _appSettings.Cipher.CypherKey;
            var initialVector = _appSettings.Cipher.InitialVector;

            key += KEY_STATIC_PAIR;
            initialVector += INITIAL_VECTOR_STATIC_PAIR;

            byte[] byteArrayToDecrypt = Convert.FromBase64String(stringToDecrypt);
            AesCryptoServiceProvider keyDecrypt = new AesCryptoServiceProvider();
            keyDecrypt.BlockSize = 128;
            keyDecrypt.KeySize = 128;
            keyDecrypt.Key = System.Text.Encoding.UTF8.GetBytes(key);
            keyDecrypt.IV = System.Text.Encoding.UTF8.GetBytes(initialVector);
            keyDecrypt.Padding = PaddingMode.PKCS7;
            keyDecrypt.Mode = CipherMode.CBC;
            ICryptoTransform crypto = keyDecrypt.CreateDecryptor(keyDecrypt.Key, keyDecrypt.IV);
            byte[] returnByteArray = crypto.TransformFinalBlock(byteArrayToDecrypt, 0, byteArrayToDecrypt.Length);
            crypto.Dispose();
            return Encoding.UTF8.GetString(returnByteArray);
        }
    }
}
