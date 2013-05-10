using System.Windows.Controls;

namespace Doit.Settings
{
	public interface ISettingsSection
	{
		string Name { get; }

		ContentControl View { get; }
		
		void UpdateSettings(SettingsData settingsData);
	}
}