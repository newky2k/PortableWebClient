using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
	/// <summary>
	/// Encryption Provider Base class for implementing a custom
	/// </summary>
	/// <seealso cref="DSoft.Portable.WebClient.Encryption.IEncryptionProvider" />
	public abstract class EncryptionProviderBase : IEncryptionProvider
    {
		/// <summary>
		/// The initialize vector
		/// </summary>
		protected string _initVector;

		/// <summary>
		/// This constant is used to determine the keysize of the encryption algorithm
		/// </summary>
		protected KeySize _keysize = KeySize.TwoFiftySix;

		/// <summary>
		/// Initializes a new instance of the <see cref="EncryptionProviderBase"/> class.
		/// </summary>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public EncryptionProviderBase(string initVector, KeySize keySize)
        {
            _initVector = initVector;
            _keysize = keySize;
        }

		/// <summary>
		/// Decrypts the bytes.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>System.Byte[].</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public virtual byte[] DecryptBytes(byte[] data, string passPhrase)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Decrypts the string.
		/// </summary>
		/// <param name="cipherText">The cipher text.</param>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public virtual string DecryptString(string cipherText, string passPhrase)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Encrypts the bytes.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>System.Byte[].</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public virtual byte[] EncryptBytes(byte[] data, string passPhrase)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Encrypts the string.
		/// </summary>
		/// <param name="plainText">The plain text.</param>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public virtual string EncryptString(string plainText, string passPhrase)
        {
            throw new NotImplementedException();
        }

    }
}
