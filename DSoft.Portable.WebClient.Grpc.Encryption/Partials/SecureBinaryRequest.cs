using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
    public partial class SecureBinaryRequest : ISecureBinaryRequest<SecurePayload, ByteString>
    {
        partial void OnConstruction()
        {
            Payload = new SecurePayload();

            
        }

        public SecureBinaryRequest(string clientVersionNo, string data) : this()
        {
            ClientVersionNo = clientVersionNo;

            Payload.Data = data;
        }

        public SecureBinaryRequest(string clientVersionNo, string data, string id) : this(clientVersionNo, data)
        {
            Id = id;
        }

        public void Validate(TimeSpan timeout)
        {
            if (Payload == null)
                throw new Exception("No payload in request");

            if (Payload.Timestamp == null || !Payload.Validate(timeout))
                throw new Exception("Payload has timed out");
        }

        public TData ExtractPayload<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            if (Payload == null)
                throw new Exception("No data");

            return Payload.Extract<TData>(passKey, initVector, keySize);
        }

        public void SetBinaryObject(byte[] data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            BinaryObject = ByteString.CopyFrom(EncryptionProviderFactory.Build(initVector, keySize).EncryptBytes(data, passKey));
        }

        public byte[] GetBinaryObject(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            if (BinaryObject == null)
                throw new Exception("Binary Object is empty");

            return EncryptionProviderFactory.Build(initVector, keySize).DecryptBytes(BinaryObject.ToByteArray(), passKey);
        }
    }
}
