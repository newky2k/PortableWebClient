using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption.Helpers
{
    public static class EncryptionHelper
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "xDRWrrCrvxioNFUv";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        private static byte[] InitVectorBytes => Encoding.UTF8.GetBytes(initVector);
        private static SymmetricAlgorithm EncryptionAlogitm => new AesManaged() { Mode = CipherMode.CBC };

        private static byte[] GetPasswordBytes(string passPhrase)
        {
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);

            return keyBytes;
        }

        private static ICryptoTransform CreateEncryptor(string passPhrase)
        {
            byte[] keyBytes = GetPasswordBytes(passPhrase);

            ICryptoTransform encryptor = EncryptionAlogitm.CreateEncryptor(keyBytes, InitVectorBytes);

            return encryptor;
        }

        private static ICryptoTransform CreateDecryptor(string passPhrase)
        {
            byte[] keyBytes = GetPasswordBytes(passPhrase);

            ICryptoTransform encryptor = EncryptionAlogitm.CreateDecryptor(keyBytes, InitVectorBytes);

            return encryptor;
        }

        private static byte[] Encrypt(string passPhrase, byte[] data)
        {
            ICryptoTransform encryptor = CreateEncryptor(passPhrase);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();

            return cipherTextBytes;
        }

        private static (byte[] data, int byteCount) Decrypt(string passPhrase, byte[] data)
        {
            ICryptoTransform decryptor = CreateDecryptor(passPhrase);

            MemoryStream memoryStream = new MemoryStream(data);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[data.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();

            return (plainTextBytes, decryptedByteCount);
        }

        #region Public Methods
        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="plainText">string to encrypt</param>
        /// <param name="passPhrase">password</param>
        /// <returns></returns>
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] cipherTextBytes = Encrypt(passPhrase, plainTextBytes);

            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        /// Encrypt a bytes[]
        /// </summary>
        /// <param name="data">byte[] to encrypt</param>
        /// <param name="passPhrase">password</param>
        /// <returns></returns>
        public static byte[] EncryptBytes(byte[] data, string passPhrase)
        {
            return Encrypt(passPhrase, data); ;
        }

        /// <summary>
		/// Decrypt a string
		/// </summary>
		/// <param name="cipherText">Encrypted text</param>
		/// <param name="passPhrase">password</param>
		/// <returns></returns>
        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            var result = Decrypt(passPhrase, cipherTextBytes);

            return Encoding.UTF8.GetString(result.data, 0, result.byteCount);
        }

        /// <summary>
        /// Decrypt byte[]
        /// </summary>
        /// <param name="data">Encrypted byte[]</param>
        /// <param name="passPhrase">password</param>
        /// <returns></returns>
        public static byte[] DecryptBytes(byte[] data, string passPhrase)
        {
            var result = Decrypt(passPhrase, data);

            return result.data;
        }

        #endregion
    }
}

