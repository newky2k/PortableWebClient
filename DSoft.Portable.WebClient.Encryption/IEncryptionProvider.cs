using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
	/// <summary>
	/// Interface IEncryptionProvider
	/// </summary>
	public interface IEncryptionProvider
    {
		/// <summary>
		/// Encrypts the string.
		/// </summary>
		/// <param name="plainText">The plain text.</param>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>System.String.</returns>
		string EncryptString(string plainText, string passPhrase);

		/// <summary>
		/// Encrypts the bytes.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>System.Byte[].</returns>
		byte[] EncryptBytes(byte[] data, string passPhrase);

		/// <summary>
		/// Decrypts the string.
		/// </summary>
		/// <param name="cipherText">The cipher text.</param>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>System.String.</returns>
		string DecryptString(string cipherText, string passPhrase);

		/// <summary>
		/// Decrypts the bytes.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="passPhrase">The pass phrase.</param>
		/// <returns>System.Byte[].</returns>
		byte[] DecryptBytes(byte[] data, string passPhrase);
    }
}
