using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using Doit.Infrastructure;

namespace Doit.Settings
{
	public class FindFilesSettingsSection : ISettingsSection
	{
		private readonly Lazy<FindFilesSettingsSectionView> _view;

		private readonly DelegateCommand _openIndexingOptionsCommand;

		public FindFilesSettingsSection(FindFilesSettings findFilesSettings)
		{
			_view = new Lazy<FindFilesSettingsSectionView>(() => new FindFilesSettingsSectionView { DataContext = this });
			_openIndexingOptionsCommand = new DelegateCommand(OpenIndexingOptions);

			Settings = findFilesSettings;
		}

		public FindFilesSettings Settings { get; set; }

		public string Name
		{
			get { return "Find Files"; }
		}

		public ContentControl View
		{
			get { return _view.Value; }
		}

		public DelegateCommand OpenIndexingOptionsCommand
		{
			get { return _openIndexingOptionsCommand; }
		}

		public void UpdateSettings(SettingsData settingsData)
		{
			settingsData.FindFilesSettings = Settings;
		}

		public void OpenIndexingOptions(object o)
		{
			var systemRootPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
			var arguments = string.Format(
				"{0},Control_RunDLL {1}", 
				Path.Combine(systemRootPath, "shell32.dll"), 
				Path.Combine(systemRootPath, "srchadmin.dll"));

			Process.Start("rundll32.exe", arguments);
		}
	}
}