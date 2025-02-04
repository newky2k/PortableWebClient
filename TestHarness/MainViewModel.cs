using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Factories;
using DSoft.Portable.WebClient.Grpc;
using DSoft.Portable.WebClient.Rest.Encryption;
using SampleApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestHarness.Services;

namespace TestHarness
{
    public class MainViewModel : ViewModel
    {
        internal string PassKey => "1234567890";
        internal string IVKey => "xRFg8Ctp1sEqWfVp";

        private static IGrpcChannelManager channelManager = new GrpcChannelManager();

		private string _baseUrl;

		public string BaseUrl
		{
			get { return _baseUrl; }
			set { _baseUrl = value; NotifyPropertyChanged(nameof(BaseUrl)); }
		}



		public ICommand GetSessionCommand
		{
			get
			{
				return new DelegateCommand(async () =>
				{
					try
					{
						if (string.IsNullOrEmpty(BaseUrl))
							throw new Exception("BaseUrl cannot be empty");

						IVProviderOptions iVProviderOptions = new()
						{
							InitVector = IVKey,
						};

						var ivProvider = IVProviderFactory.Create(iVProviderOptions);

                        SecureRestApiClientOptions options = new()
						{
							KeySize = KeySize.TwoFiftySix,
						};

                        SessionService sessionService = new(options, ivProvider);

                        var session = await sessionService.GenerateSessionTokenAsync(PassKey);

					}
					catch (Exception ex)
					{

						NotifyErrorOccurred(ex);
					}
				});
			}
		}



		public ICommand TestGrpcCommand
		{
			get
			{
				return new DelegateCommand(async () =>
				{
					try
					{
						GrpcClientOptions grpcOptions = new()
						{
							GrpcMode = HttpMode.Http_2_0
						};

						var servClient = new SampleServiceClient(channelManager, grpcOptions);

						var result = await servClient.FindAsync(1);


					}
					catch (Exception ex)
					{

						NotifyErrorOccurred(ex);
					}
				});
			}
		}




		public MainViewModel()
		{

		}
    }
}
