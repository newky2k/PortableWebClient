using DSoft.Portable.WebClient.Encryption.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{

    /// <summary>
    /// Send a payload and an encrypted binary
    /// </summary>
    public class SecureBinaryRequest : SecureRequest
    {

        public byte[] BinaryObject { get; set; }


        public void SetBinaryObject(byte[] data, string passKey)
        {
            BinaryObject = EncryptionHelper.EncryptBytes(data, passKey);
        }

        public byte[] GetBinaryObject(string passKey)
        {
            if (BinaryObject == null)
                throw new Exception("Binary Object is empty");

            return EncryptionHelper.DecryptBytes(BinaryObject, passKey);
        }

        public SecureBinaryRequest()
        {

        }
    }
}
