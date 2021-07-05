using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
    public partial class SecurePayload : ISecurePayload
    {
        partial void OnConstruction()
        {
            Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow);
        }

        public SecurePayload(object dataValue, string passKey) : this()
        {
            Data = Google.Protobuf.ByteString.CopyFromUtf8(PayloadManager.EncryptPayload(dataValue, passKey));
        }

        public SecurePayload(string passKey) : this(EmptyPayload.Empty, passKey)
        {

        }

        public bool Validate(TimeSpan timeSpan)
        {
            var timeStamp = Timestamp.ToDateTime();

            var diff = DateTime.Now - timeStamp;

            return (diff < timeSpan);
        }

        public T Extract<T>(string passKey)
        {
            return PayloadManager.DecryptPayload<T>(Data.ToStringUtf8(), passKey);
        }

        
    }
}
