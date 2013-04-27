using System;
using System.Windows;

namespace Doit
{
	public partial class App
	{
		public bool StartHidden { get; set; }

		private void AppOnStartup(object sender, StartupEventArgs e)
		{
			StartHidden = e.Args.Length == 1 && string.Equals(e.Args[0], "-StartHidden", StringComparison.InvariantCultureIgnoreCase);

			var mainWindow = new MainWindow();

			mainWindow.Show();
		}
	}
}