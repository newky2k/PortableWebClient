using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
	/// <summary>
	/// Empty variable legnth payload object for response without data
	/// </summary>
	public class EmptyPayload
    {
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public string Data { get; set; }

		/// <summary>
		/// Gets an instance of EmptyPayload with a random payload
		/// </summary>
		/// <value>The empty.</value>
		public static EmptyPayload Empty
        {
            get
            {

                var rnd = new Random();
                var size = rnd.Next(5, 20);

                var data = new byte[size];

                var rng = RandomNumberGenerator.Create();
                rng.GetBytes(data);

                var str = Encoding.UTF8.GetString(data);

                return new EmptyPayload()
                {
                    Data = str,
                };
            }
        }
    }
}
