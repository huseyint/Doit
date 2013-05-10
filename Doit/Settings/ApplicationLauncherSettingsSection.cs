using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Doit.Infrastructure;
using Ookii.Dialogs.Wpf;

namespace Doit.Settings
{
	public class ApplicationLauncherSettingsSection : ISettingsSection, INotifyPropertyChanged
	{
		private readonly Lazy<ApplicationLauncherSettingsSectionView> _view;

		private readonly DelegateCommand _addIndexLocationCommand;
		
		private readonly DelegateCommand _removeIndexLocationCommand;

		private string _selectedIndexLocation;

		public ApplicationLauncherSettingsSection(ApplicationLauncherSettings applicationLauncherSettings)
		{
			_view = new Lazy<ApplicationLauncherSettingsSectionView>(() => new ApplicationLauncherSettingsSectionView { DataContext = this });
			_addIndexLocationCommand = new DelegateCommand(AddIndexLocation);
			_removeIndexLocationCommand = new DelegateCommand(RemoveIndexLocation, CanRemoveIndexLocation);

			Settings = applicationLauncherSettings;

			IndexLocations = new ObservableCollection<string>(Settings.IndexLocations);
			SelectedIndexLocation = IndexLocations.FirstOrDefault();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ApplicationLauncherSettings Settings { get; set; }

		public ObservableCollection<string> IndexLocations { get; set; }

		public string SelectedIndexLocation
		{
			get
			{
				return _selectedIndexLocation;
			}

			set
			{
				_selectedIndexLocation = value;

				OnPropertyChanged();
			}
		}

		public string Name
		{
			get { return "Application Launcher"; }
		}

		public ContentControl View
		{
			get { return _view.Value; }
		}

		public DelegateCommand AddIndexLocationCommand
		{
			get { return _addIndexLocationCommand; }
		}

		public DelegateCommand RemoveIndexLocationCommand
		{
			get { return _removeIndexLocationCommand; }
		}

		public void UpdateSettings(SettingsData settingsData)
		{
			Settings.IndexLocations = IndexLocations.ToArray();

			settingsData.ApplicationLauncherSettings = Settings;
		}

		public void AddIndexLocation(object o)
		{
			var dialog = new VistaFolderBrowserDialog
			{
				Description = "Select Index Location",
				UseDescriptionForTitle = true,
			};

			var result = dialog.ShowDialog();

			if (result.HasValue && result.Value)
			{
				if (!IndexLocations.Contains(dialog.SelectedPath))
				{
					IndexLocations.Add(dialog.SelectedPath); 
				}

				SelectedIndexLocation = dialog.SelectedPath;
			}
		}

		public void RemoveIndexLocation(object o)
		{
			if (CanRemoveIndexLocation())
			{
				using (var dialog = new TaskDialog())
				{
					dialog.WindowTitle = "Remove Index Location";
					dialog.MainInstruction = "Are you sure you want to remove the selected index location?";
					dialog.Content = "This location will not be indexed anymore.";
					dialog.ButtonStyle = TaskDialogButtonStyle.CommandLinks;

					var removeButton = new TaskDialogButton("Remove") { CommandLinkNote = "Yes, remove it" };
					dialog.Buttons.Add(removeButton);
					dialog.Buttons.Add(new TaskDialogButton(ButtonType.Cancel));
					var result = dialog.ShowDialog();

					if (result == removeButton)
					{
						IndexLocations.Remove(SelectedIndexLocation);
					}
				}
			}
		}

		public bool CanRemoveIndexLocation(object o = null)
		{
			return SelectedIndexLocation != null;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}