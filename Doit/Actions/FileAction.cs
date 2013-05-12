using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Doit.ActionResults;
using Doit.Infrastructure;
using Doit.Native;

namespace Doit.Actions
{
	public class FileAction : IAction
	{
		protected readonly string _path;
		
		protected readonly string[] _paths;

		protected BitmapSource _icon;

		public FileAction(string path)
		{
			if (!IsValidFile(path))
			{
				throw new ArgumentException("Invalid file path.", "path");
			}

			_path = path;

			if (_path != null)
			{
				ushort uicon;
				var handle = NativeMethods.ExtractAssociatedIcon(new WindowInteropHelper(Application.Current.MainWindow).Handle, new StringBuilder(_path), out uicon);

				try
				{
					_icon = Imaging.CreateBitmapSourceFromHIcon(handle, new Int32Rect(0, 0, 32, 32), BitmapSizeOptions.FromEmptyOptions());
				}
				finally
				{
					NativeMethods.DestroyIcon(handle);
				}
			}

			Hint = Path.GetDirectoryName(_path);
		}

		public FileAction(string[] paths)
		{
			if (paths.Any(p => !IsValidFile(p)))
			{
				throw new ArgumentException("Invalid file path exists.", "paths");
			}

			_paths = paths;

			_icon = new BitmapImage(new Uri("pack://application:,,,/Images/Files32.png"));
		}

		public Type ResultType
		{
			get { return typeof(FileActionResult); }
		}

		public virtual string Text
		{
			get
			{
				if (!string.IsNullOrEmpty(_path))
				{
					return Path.GetFileName(_path);
				}

				return string.Format("{0} file(s)", _paths.Length);
			}
		}

		public string Hint { get; set; }

		public virtual ImageSource Icon
		{
			get { return _icon; }
		}

		public virtual ActionResult Execute(ExecutionContext context)
		{
			if (context.HasNextAction)
			{
				return new FileActionResult { Paths = _paths ?? new[] { _path } };
			}

			if (!string.IsNullOrEmpty(_path))
			{
				var startInfo = new ProcessStartInfo(_path);

				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					startInfo.Verb = "runas";
				}

				Process.Start(startInfo);

				return ActionResult.Default;
			}

			return ActionResult.PreventHide;
		}

		public bool IsFallbackMatch { get; set; }

		public override string ToString()
		{
			return Text;
		}

		private static bool IsValidFile(string path)
		{
			return File.Exists(path) || Directory.Exists(path);
		}
	}
}