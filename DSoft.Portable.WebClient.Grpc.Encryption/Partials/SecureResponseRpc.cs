using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
    public partial class SecureResponseRpc
    {

        partial void OnConstruction()
        {
            Payload = new SecurePayloadRpc()
            {
                Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
            };
        }

        public void SetPayLoad(string data)
        {
            Payload.Data = Google.Protobuf.ByteString.CopyFromUtf8(data);
        }

    }
}
