﻿using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
    public partial class SecureRequest : ISecureRequest<SecurePayload>
    {
        partial void OnConstruction()
        {
            Payload = new SecurePayload();
        }

        public SecureRequest(string clientVersionNo, string data) : base()
        {
            ClientVersionNo = clientVersionNo;

            Payload.Data = data;
        }

        public SecureRequest(string clientVersionNo, string data, string id) : this(clientVersionNo, data)
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
    }
}
