using System.Windows;
using System.Windows.Input;

namespace Doit.Settings
{
	public partial class GeneralSettingsSectionView
	{
		public GeneralSettingsSectionView()
		{
			InitializeComponent();
		}

		private GeneralSettings Settings
		{
			get { return ((GeneralSettingsSection)DataContext).Settings; }
		}

		private void HotkeyTextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;

			if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl || e.Key == Key.LeftAlt || e.Key == Key.RightAlt || e.Key == Key.System || e.Key == Key.LeftShift || e.Key == Key.RightShift)
			{
				return;
			}

			HotkeyTextBox.Text = e.Key.ToString();
			Settings.Hotkey = e.Key;
		}

		private void GeneralSettingsSectionViewOnLoaded(object sender, RoutedEventArgs e)
		{
			HotkeyTextBox.Text = Settings.Hotkey.ToString();
		}
	}
}