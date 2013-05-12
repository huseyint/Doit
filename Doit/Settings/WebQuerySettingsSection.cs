using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace Doit.Settings
{
	public class WebQuerySettingsSection : ISettingsSection
	{
		private readonly Lazy<WebQuerySettingsSectionView> _view;

		public WebQuerySettingsSection(WebQuerySettings[] webQuerySettings)
		{
			_view = new Lazy<WebQuerySettingsSectionView>(() => new WebQuerySettingsSectionView { DataContext = this });
			
			Queries = new ObservableCollection<WebQuerySettings>(webQuerySettings);
		}

		public ObservableCollection<WebQuerySettings> Queries { get; set; }

		public string Name
		{
			get { return "Web Queries"; }
		}

		public ContentControl View
		{
			get { return _view.Value; }
		}

		public void UpdateSettings(SettingsData settingsData)
		{
			settingsData.WebQuerySettings = Queries.ToArray();
		}
	}
}