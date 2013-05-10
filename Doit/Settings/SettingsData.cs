using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Doit.Settings
{
	[XmlType]
	public class SettingsData
	{
		private const string FileName = ".\\Settings.xml";

		public SettingsData()
		{
			GeneralSettings = new GeneralSettings();
			ApplicationLauncherSettings = new ApplicationLauncherSettings();
		}

		public GeneralSettings GeneralSettings { get; set; }

		public ApplicationLauncherSettings ApplicationLauncherSettings { get; set; }

		public static SettingsData Load()
		{
			if (!File.Exists(FileName))
			{
				return new SettingsData();
			}

			var serializer = new XmlSerializer(typeof(SettingsData));

			using (var stream = File.OpenRead(FileName))
			{
				return (SettingsData)serializer.Deserialize(stream);
			}
		}

		public static void Save(SettingsData settingsData)
		{
			var settings = new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				Indent = true
			};

			var serializer = new XmlSerializer(typeof(SettingsData));

			using (var stream = File.Open(FileName, FileMode.Create, FileAccess.Write))
			using (var writer = XmlWriter.Create(stream, settings))
			{
				serializer.Serialize(writer, settingsData);
			}
		}
	}
}