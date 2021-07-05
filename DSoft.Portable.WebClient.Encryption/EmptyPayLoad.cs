using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public class EmptyPayload
    {
        public string Data { get; set; }

        public static EmptyPayload Empty
        {
            get
            {
                return new EmptyPayload()
                {
                    Data = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                };
            }
        }
    }
}
