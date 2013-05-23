using System.ComponentModel;
using System.Xml.Serialization;

namespace Doit.Settings
{
	[XmlType]
	public class FindFilesSettings
	{
		public FindFilesSettings()
		{
			IsEnabled = true;
			IsFallback = true;
		}

		[XmlAttribute]
		[DefaultValue(true)]
		public bool IsEnabled { get; set; }

		[XmlAttribute]
		[DefaultValue(true)]
		public bool IsFallback { get; set; }
	}
}