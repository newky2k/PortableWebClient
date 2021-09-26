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

        public byte[] BinaryObject { get; set; }


        public void SetBinaryObject(byte[] data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            BinaryObject = EncryptionProviderFactory.Build(initVector, keySize).EncryptBytes(data, passKey);
        }

        public byte[] GetBinaryObject(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            if (BinaryObject == null)
                throw new Exception("Binary Object is empty");

            return EncryptionProviderFactory.Build(initVector, keySize).DecryptBytes(BinaryObject, passKey);
        }

        public SecureBinaryRequest()
        {

        }
    }
}
