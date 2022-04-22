using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Grpc
{
	public class GrpcClientOptions
	{
		public HttpMode GrpcMode { get; set; } = HttpMode.Http_1_1;


		public bool DisableSSLCertValidation { get; set; } = false;

	}
}
