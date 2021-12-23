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



		public MainViewModel()
		{

		}
    }
}
