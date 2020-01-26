using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.Server.Security.Core
{
    public class Session : ISession<Guid,string>
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime Timestamp { get; set; }

        public DateTime Expires { get; set; }


        public Session()
        {
            Token = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
            Expires = Timestamp.AddHours(1);  // Session is very temporary
        }
    }
}
