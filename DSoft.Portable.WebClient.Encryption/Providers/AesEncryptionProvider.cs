using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption.Providers
{

	/// <summary>
	/// Authentication provider using Aes CBC
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.EncryptionProviderBase" />
	internal class AesEncryptionProvider : EncryptionProviderBase
    {

		/// <summary>
		/// Gets the initialize vector bytes.
		/// </summary>
		/// <value>The initialize vector bytes.</value>
		private byte[] InitVectorBytes => Encoding.UTF8.GetBytes(_initVector);


		/// <summary>
		/// Gets the encryption algorithm.
		/// </summary>
		/// <value>The encryption algorithm.</value>
		private SymmetricAlgorithm EncryptionAlgorithm => new AesManaged() { Mode = CipherMode.CBC };


		/// <summary>
		/// Initializes a new instance of the <see cref="AesEncryptionProvider"/> class.
		/// </summary>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public AesEncryptionProvider(string initVector, KeySize keySize) : base(initVector, keySize)
        {

        }

		#region Private Methods

		/// <summary>
		/// Gets the password bytes.
		/// </summary>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>System.Byte[].</returns>
		private byte[] GetPasswordBytes(string passPhrase)
        {
            var password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes((int)_keysize / 8);

            return keyBytes;
        }

		/// <summary>
		/// Creates the encryptor.
		/// </summary>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>ICryptoTransform.</returns>
		private ICryptoTransform CreateEncryptor(string passPhrase)
        {
            byte[] keyBytes = GetPasswordBytes(passPhrase);

            var encryptor = EncryptionAlgorithm.CreateEncryptor(keyBytes, InitVectorBytes);

            return encryptor;
        }

		/// <summary>
		/// Creates the decryptor.
		/// </summary>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>ICryptoTransform.</returns>
		private ICryptoTransform CreateDecryptor(string passPhrase)
        {
            byte[] keyBytes = GetPasswordBytes(passPhrase);

            ICryptoTransform encryptor = EncryptionAlgorithm.CreateDecryptor(keyBytes, InitVectorBytes);

            return encryptor;
        }

		/// <summary>
		/// Encrypts the specified pass phrase.
		/// </summary>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <param name="data">The data.</param>
		/// <returns>System.Byte[].</returns>
		private byte[] Encrypt(string passPhrase, byte[] data)
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

		/// <summary>
		/// Decrypts the specified pass phrase.
		/// </summary>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <param name="data">The data.</param>
		/// <returns>System.ValueTuple&lt;System.Byte[], System.Int32&gt;.</returns>
		private (byte[] data, int byteCount) Decrypt(string passPhrase, byte[] data)
        {
            ICryptoTransform decryptor = CreateDecryptor(passPhrase);

            MemoryStream memoryStream = new MemoryStream(data);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] plainTextBytes;

            var binaryReader = new BinaryReader(cryptoStream);

            plainTextBytes = binaryReader.ReadBytes(data.Length);

            binaryReader.Close();

            memoryStream.Close();
            cryptoStream.Close();

            return (plainTextBytes, plainTextBytes.Length);
        }

		#endregion

		#region Public Methods
		/// <summary>
		/// Encrypt a string
		/// </summary>
		/// <param name="plainText">string to encrypt</param>
		/// <param name="passPhrase">password</param>
		/// <returns>System.String.</returns>
		public override string EncryptString(string plainText, string passPhrase)
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
		/// <returns>System.Byte[].</returns>
		public override byte[] EncryptBytes(byte[] data, string passPhrase)
        {
            return Encrypt(passPhrase, data); ;
        }

		/// <summary>
		/// Decrypt a string
		/// </summary>
		/// <param name="cipherText">Encrypted text</param>
		/// <param name="passPhrase">password</param>
		/// <returns>System.String.</returns>
		public override string DecryptString(string cipherText, string passPhrase)
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
		/// <returns>System.Byte[].</returns>
		public override byte[] DecryptBytes(byte[] data, string passPhrase)
        {
            var result = Decrypt(passPhrase, data);

            return result.data;
        }

        #endregion
    }
}
