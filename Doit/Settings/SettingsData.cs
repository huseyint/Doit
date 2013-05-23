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
			WebQuerySettings = new[]
			{
				new WebQuerySettings
				{
					IsEnabled = true,
					IsFallback = true,
					Verb = "google",
					Query = "https://www.google.com/search?q={0}",
					IconPath = "pack://application:,,,/Images/Google32.png",
				}, 
				new WebQuerySettings
				{
					IsEnabled = true,
					Verb = "bing",
					Query = "http://www.bing.com/search?q={0}",
					IconPath = "pack://application:,,,/Images/Bing32.png",
				}, 
				new WebQuerySettings
				{
					IsEnabled = true,
					Verb = "duck",
					Query = "https://duckduckgo.com/?q={0}",
					IconPath = "pack://application:,,,/Images/Duck32.png",
				}, 
				new WebQuerySettings
				{
					IsEnabled = true,
					Verb = "wiki",
					Query = "http://en.wikipedia.org/wiki/Special:Search?search={0}&go=Go",
					IconPath = "pack://application:,,,/Images/Wiki32.png",
				}, 
				new WebQuerySettings
				{
					IsEnabled = true,
					Verb = "imdb",
					Query = "http://www.imdb.com/find?s=all&q={0}",
					IconPath = "pack://application:,,,/Images/Imdb32.png",
				}, 
			};
			FindFilesSettings = new FindFilesSettings();
		}

		public GeneralSettings GeneralSettings { get; set; }

		public ApplicationLauncherSettings ApplicationLauncherSettings { get; set; }

		[XmlArrayItem("WebQuery")]
		public WebQuerySettings[] WebQuerySettings { get; set; }

		public FindFilesSettings FindFilesSettings { get; set; }

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