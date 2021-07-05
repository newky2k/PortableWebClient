using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
    public partial class SecureResponse : ISecureResponse<SecurePayload>
    {

        partial void OnConstruction()
        {
            Payload = new SecurePayload()
            {
                Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
            };
        }

        
        public void SetPayLoad(string data)
        {
            Payload.Data = Google.Protobuf.ByteString.CopyFromUtf8(data);
        }

        public void SetPayLoad(object data, string passKey) => SetPayLoad(PayloadManager.EncryptPayload(data, passKey));

        public TData Extract<TData>(string passKey)
        {
            if (Payload == null)
                throw new Exception("No data");

            return Payload.Extract<TData>(passKey);
        }

    }
}
