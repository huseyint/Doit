using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Doit.Infrastructure;
using Doit.Native;
using Doit.Settings;
using MessageBox = System.Windows.MessageBox;

namespace Doit
{
	public partial class App : ISingleInstanceApp
	{
		private const string AppId = "C3C0A016-9C2C-4C9F-905F-73E458F89C59";

		private HotKey _hotkey;

		public bool StartHidden { get; set; }

		[STAThread]
		public static void Main()
		{
			if (SingleInstance<App>.InitializeAsFirstInstance(AppId))
			{
				var application = new App();

				application.InitializeComponent();
				application.Run();

				// Allow single instance code to perform cleanup operations
				SingleInstance<App>.Cleanup();
			}
		}

		void ISingleInstanceApp.SignalExternalCommandLineArgs(IList<string> args)
		{
			var mainWindow = (MainWindow)Current.MainWindow;
			mainWindow.ShowMe();
		}

		public void SetHotkey()
		{
			if (_hotkey != null)
			{
				_hotkey.Unregister();
				_hotkey.Dispose();
			}

			var settings = SettingsData.Load().GeneralSettings;

			var modifierKeys = default(ModifierKeys);

			if (settings.HotkeyControl)
			{
				modifierKeys |= ModifierKeys.Control;
			}
			
			if (settings.HotkeyAlt)
			{
				modifierKeys |= ModifierKeys.Alt;
			}

			if (settings.HotkeyShift)
			{
				modifierKeys |= ModifierKeys.Shift;
			}

			var key = (Keys)KeyInterop.VirtualKeyFromKey(settings.Hotkey);

			_hotkey = new HotKey(modifierKeys, key, Current.MainWindow);

			var isRegistered = _hotkey.Register();

			if (isRegistered)
			{
				_hotkey.HotKeyPressed += OnHotKeyPressed;
			}
			else
			{
				MessageBox.Show("Can't register hot key!", "Doit App - Register Hotkey", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void AppOnStartup(object sender, StartupEventArgs e)
		{
			StartHidden = e.Args.Length == 1 && string.Equals(e.Args[0], "-StartHidden", StringComparison.InvariantCultureIgnoreCase);

			var mainWindow = new MainWindow();

			SetHotkey();

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