using System;

namespace PortableClient.Models
{
	public class SessionDto
	{
		public Guid Id { get; set; }

		public string Token { get; set; }

		public DateTime Timestamp { get; set; }

		public DateTime Expires { get; set; }

	}
}
