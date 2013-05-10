using System;
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

		public bool IsEnabled { get; set; }
		
		public bool IsFallback { get; set; }

		[XmlArrayItem("IndexLocation")]
		public string[] IndexLocations { get; set; }
	}
}