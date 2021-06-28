using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc.Encryption
{
    public partial class SecureRequestRpc
    {
        partial void OnConstruction()
        {
            Payload = new SecurePayloadRpc();
        }

        public SecureRequestRpc(string clientVersionNo, string data) : base()
        {
            ClientVersionNo = clientVersionNo;

            Payload.Data = Google.Protobuf.ByteString.CopyFromUtf8(data);
        }

        public SecureRequestRpc(string clientVersionNo, string data, string id) : this(clientVersionNo, data)
        {
            Id = id;
        }
    }
}
