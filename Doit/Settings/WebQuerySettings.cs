using System.Xml.Serialization;

namespace Doit.Settings
{
	[XmlType]
	public class WebQuerySettings
	{
		public WebQuerySettings()
		{
			IsEnabled = true;
		}

		[XmlAttribute]
		public bool IsEnabled { get; set; }

		[XmlAttribute]
		public bool IsFallback { get; set; }

		public string Verb { get; set; }

		public string Query { get; set; }

		public string IconPath { get; set; }
	}
}