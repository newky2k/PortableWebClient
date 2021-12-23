using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Mvvm.Ui;
using System.Threading.Tasks;
using System.Windows;

namespace TestHarness
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			MvvmManager.Init();

		}
	}
}
