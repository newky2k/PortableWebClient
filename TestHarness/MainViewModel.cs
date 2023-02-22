using DSoft.Portable.WebClient.Grpc;
using SampleApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestHarness.Client;

namespace TestHarness
{
    public class MainViewModel : ViewModel
    {
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

						using (var client = new TestApiClient(BaseUrl))
						{
							var session = await client.Session.GenerateSessionTokenAsync();

						}
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
						var webClient = new WebClient(BaseUrl);

						var servClient = new SampleServiceClient(webClient, channelManager, HttpMode.Http_2_0);

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
