using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc
{
	public class GrpcClientOptions
	{
		/// <summary>
		/// Http mode for Grpc channels.  1.1 will use Grpc-web, 2.0 will use HTTP/2
		/// </summary>
		public HttpMode GrpcMode { get; set; } = HttpMode.Http_1_1;

		/// <summary>
		/// Disable the SSL Certificate validation.  Ignored if ServerCertificateCustomValidationCallback is set
		/// </summary>
		public bool DisableSSLCertValidation { get; set; } = false;

		/// <summary>
		/// Callback function to override defualt SSL Certificate validatiion.  If set the DisableSSLCertValidation is ignored
		/// </summary>
		public Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> ServerCertificateCustomValidationCallback { get; set; }

	}
}
