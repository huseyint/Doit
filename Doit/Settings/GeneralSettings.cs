using System.Windows.Input;
using System.Xml.Serialization;

namespace Doit.Settings
{
	[XmlType]
	public class GeneralSettings
	{
		public GeneralSettings()
		{
			HotkeyAlt = true;
			Hotkey = Key.Space;
		}

		public bool HotkeyControl { get; set; }
		
		public bool HotkeyAlt { get; set; }

		public bool HotkeyShift { get; set; }

		public Key Hotkey { get; set; }
	}
}