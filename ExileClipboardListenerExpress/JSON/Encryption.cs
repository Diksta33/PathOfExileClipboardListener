using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ExileClipboardListener.JSON
{
    public static class StringCipher
    {
        //This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        //This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        //32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string InitVector = "hsd3fkjsd5f7ds1f";
        private const string SaltText = "The Butcher";
        private const string PassPhrase = "Fresh Meat";

        //This constant is used to determine the keysize of the encryption algorithm.
        private const int Keysize = 256;

        public static string Encrypt(string plainText)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] saltKey = Encoding.UTF8.GetBytes(SaltText);
            var password = new Rfc2898DeriveBytes(PassPhrase, saltKey);
            byte[] keyBytes = password.GetBytes(Keysize / 8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string cipherText)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] saltKey = Encoding.UTF8.GetBytes(SaltText);
            var password = new Rfc2898DeriveBytes(PassPhrase, saltKey);
            byte[] keyBytes = password.GetBytes(Keysize / 8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }

}