using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public class EmptyPayLoad
    {
        public string Data { get; set; }

        public static EmptyPayLoad Empty
        {
            get
            {
                return new EmptyPayLoad()
                {
                    Data = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                };
            }
        }
    }
}
