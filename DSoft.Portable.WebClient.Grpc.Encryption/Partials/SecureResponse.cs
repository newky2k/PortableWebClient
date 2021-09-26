﻿using DSoft.Portable.WebClient.Encryption;
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
            Success = true;

            Payload = new SecurePayload()
            {
                Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
            };
        }

        
        public void SetPayload(string data)
        {
            Payload.Data = data;
        }

        public void SetPayload(object data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix) => SetPayload(PayloadManager.EncryptPayload(data, passKey, initVector, keySize));

        public TData Extract<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            if (Payload == null)
                throw new Exception("No data");

            return Payload.Extract<TData>(passKey, initVector, keySize);
        }

    }
}
