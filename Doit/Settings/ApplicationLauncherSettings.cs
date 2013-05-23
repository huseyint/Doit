using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Doit.Settings
{
	[XmlType]
	public class ApplicationLauncherSettings
	{
		public ApplicationLauncherSettings()
		{
			IndexLocations = new[]
			{
				Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu),
				Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
			};

			IsEnabled = true;
			IsFallback = true;
		}

		[XmlAttribute]
		[DefaultValue(true)]
		public bool IsEnabled { get; set; }
		
		[XmlAttribute]
		[DefaultValue(true)]
		public bool IsFallback { get; set; }

		[XmlArrayItem("IndexLocation")]
		public string[] IndexLocations { get; set; }
	}
}