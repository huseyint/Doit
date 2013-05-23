using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Doit.Settings
{
	public partial class SettingsDialog : INotifyPropertyChanged
	{
		private readonly ISettingsSection[] _settingsSections;

		private ISettingsSection _selectedSettingsSection;

		public SettingsDialog()
		{
			InitializeComponent();

			var settingsData = SettingsData.Load();

			_settingsSections = new ISettingsSection[]
			{
				new GeneralSettingsSection(settingsData.GeneralSettings),
				new ApplicationLauncherSettingsSection(settingsData.ApplicationLauncherSettings),
				new WebQuerySettingsSection(settingsData.WebQuerySettings), 
				new FindFilesSettingsSection(settingsData.FindFilesSettings), 
			};

			SelectedSettingsSection = _settingsSections.FirstOrDefault();

			DataContext = this;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ISettingsSection[] SettingsSections
		{
			get { return _settingsSections; }
		}

		public ISettingsSection SelectedSettingsSection
		{
			get
			{
				return _selectedSettingsSection;
			}

			set
			{
				_selectedSettingsSection = value;

				OnPropertyChanged(); 
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private void AcceptButtonClick(object sender, RoutedEventArgs e)
		{
			var settingsData = new SettingsData();

			foreach (var section in _settingsSections)
			{
				section.UpdateSettings(settingsData);
			}

			SettingsData.Save(settingsData);

			DialogResult = true;
			Close();
		}

		private void CancelButtonClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}