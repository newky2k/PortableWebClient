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

        public SecureBinaryRequest(string clientVersionNo, string data) : base()
        {
            ClientVersionNo = clientVersionNo;

            Payload.Data = Google.Protobuf.ByteString.CopyFromUtf8(data);
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

        public TData ExtractPayload<TData>(string passKey)
        {
            if (Payload == null)
                throw new Exception("No data");

            return Payload.Extract<TData>(passKey);
        }

        public void SetBinaryObject(byte[] data, string passKey)
        {
            BinaryObject = ByteString.CopyFrom(EncryptionHelper.EncryptBytes(data, passKey));
        }

        public byte[] GetBinaryObject(string passKey)
        {
            if (BinaryObject == null)
                throw new Exception("Binary Object is empty");

            return EncryptionHelper.DecryptBytes(BinaryObject.ToByteArray(), passKey);
        }
    }
}
