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

        public SecurePayload(object dataValue, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this()
        {
            Data = PayloadManager.EncryptPayload(dataValue, passKey, initVector, keySize);
        }

        public SecurePayload(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(EmptyPayload.Empty, passKey, initVector, keySize)
        {

        }

        public bool Validate(TimeSpan timeSpan)
        {
            var timeStamp = Timestamp.ToDateTime();

            var diff = DateTime.Now - timeStamp;

            return (diff < timeSpan);
        }

        public T Extract<T>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            return PayloadManager.DecryptPayload<T>(Data, passKey, initVector, keySize);
        }

        
    }
}
