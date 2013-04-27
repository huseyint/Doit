using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Doit.Native;
using MessageBox = System.Windows.MessageBox;

namespace Doit
{
	public partial class App
	{
		public bool StartHidden { get; set; }

		private void AppOnStartup(object sender, StartupEventArgs e)
		{
			StartHidden = e.Args.Length == 1 && string.Equals(e.Args[0], "-StartHidden", StringComparison.InvariantCultureIgnoreCase);

			var mainWindow = new MainWindow();

			var hotkey = new HotKey(ModifierKeys.Alt, Keys.Space, mainWindow);

			var isRegistered = hotkey.Register();

			if (isRegistered)
			{
				hotkey.HotKeyPressed += OnHotKeyPressed;
			}
			else
			{
				MessageBox.Show("Can't register hot key!", "Doit App - Register Hotkey", MessageBoxButton.OK, MessageBoxImage.Warning);
			}

			mainWindow.Show();
		}

		private void OnHotKeyPressed(HotKey k)
		{
			var mainWindow = (MainWindow)Current.MainWindow;

			if (mainWindow.IsVisible)
			{
				mainWindow.HideMe();
			}
			else
			{
				mainWindow.ShowMe();
			}
		}
	}
}