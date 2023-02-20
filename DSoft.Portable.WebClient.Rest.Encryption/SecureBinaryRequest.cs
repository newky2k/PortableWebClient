using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{

	/// <summary>
	/// Send a payload and an encrypted binary
	/// </summary>
	public class SecureBinaryRequest : SecureRequest, ISecureBinaryRequest<SecurePayload, byte[]>
    {

		/// <summary>
		/// Gets or sets the binary object.
		/// </summary>
		/// <value>The binary object.</value>
		public byte[] BinaryObject { get; set; }


		/// <summary>
		/// Sets the binary object.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		public void SetBinaryObject(byte[] data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            BinaryObject = EncryptionProviderFactory.Build(initVector, keySize).EncryptBytes(data, passKey);
        }

		/// <summary>
		/// Gets the binary object.
		/// </summary>
		/// <param name="passKey">The pass key.</param>
		/// <param name="initVector">The initialize vector.</param>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>System.Byte[].</returns>
		/// <exception cref="System.Exception">Binary Object is empty</exception>
		public byte[] GetBinaryObject(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            if (BinaryObject == null)
                throw new Exception("Binary Object is empty");

            return EncryptionProviderFactory.Build(initVector, keySize).DecryptBytes(BinaryObject, passKey);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureBinaryRequest"/> class.
		/// </summary>
		public SecureBinaryRequest()
        {

        }
    }
}
