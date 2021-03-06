﻿using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
    public class SecureRequest : RequestBase, ISecureRequest<SecurePayload>
    {
        public string Id { get; set; }

        public string TokenId { get; set; }

        public SecurePayload Payload { get; set; }

        public string Context { get; set; }

        public SecureRequest()
        {
            Payload = new SecurePayload();
        }

        public SecureRequest(string passKey)
        {
            Payload = new SecurePayload(passKey);
        }

        public SecureRequest(string passKey, object data)
        {
            Payload = new SecurePayload(data, passKey);
        }

        public SecureRequest(string passKey, DateTime timestamp) : this(passKey)
        {
            Payload.Timestamp = timestamp;
        }

        public SecureRequest(string passKey, string clientVersionNumber) : this(passKey)
        {
            ClientVersionNo = clientVersionNumber;
        }

        public SecureRequest(string passKey, string clientVersionNumber, DateTime timestamp) : this(passKey, clientVersionNumber)
        {
            Payload.Timestamp = timestamp;
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
