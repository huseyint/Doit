using System;
using System.Windows.Controls;

namespace Doit.Settings
{
	public class GeneralSettingsSection : ISettingsSection
	{
		private readonly Lazy<GeneralSettingsSectionView> _view;

		public GeneralSettingsSection(GeneralSettings generalSettings)
		{
			_view = new Lazy<GeneralSettingsSectionView>(() => new GeneralSettingsSectionView { DataContext = this });
			Settings = generalSettings;
		}

		public GeneralSettings Settings { get; set; }

		public string Name
		{
			get { return "General"; }
		}

		public ContentControl View
		{
			get { return _view.Value; }
		}

		public void UpdateSettings(SettingsData settingsData)
		{
			settingsData.GeneralSettings = Settings;
		}
	}
}