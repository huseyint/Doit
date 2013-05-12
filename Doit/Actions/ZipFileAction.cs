using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Doit.ActionResults;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public class ZipFileAction : IAction
	{
		private static readonly BitmapSource _icon;

		static ZipFileAction()
		{
			_icon = new BitmapImage(new Uri("pack://application:,,,/Images/Zip32.png"));
		}

		public string Text
		{
			get { return "ZIP file"; }
		}

		public ImageSource Icon
		{
			get { return _icon; }
		}

		public string Hint
		{
			get { return "ZIPs the selected file(s)"; }
		}

		public Type ResultType
		{
			get { return typeof(FileActionResult); }
		}

		public bool IsFallbackMatch { get; set; }

		public ActionResult Execute(ExecutionContext context)
		{
			var fileActionResult = (FileActionResult)context.LastActionResult;

			var paths = fileActionResult.Paths;

			if (paths == null || paths.Length == 0)
			{
				return ActionResult.Default;
			}

			var path = Path.GetDirectoryName(paths[0]);
			var archiveFileName = paths.Length == 1 ? paths[0] + ".zip" : Path.Combine(path, "archive.zip");

			using (var newFile = ZipFile.Open(archiveFileName, ZipArchiveMode.Create))
			{
				foreach (var s in paths)
				{
					newFile.CreateEntryFromFile(s, Path.GetFileName(s));
				}
			}

			return new FileActionResult { Paths = new[] { archiveFileName } };
		}

		public override string ToString()
		{
			return Text;
		}
	}
}