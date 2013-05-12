using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using Doit.ActionResults;
using Doit.Infrastructure;
using Doit.Native;

namespace Doit.Actions
{
	public class RunApplicationAction : IAction
	{
		private readonly string _path;
		private readonly string _text;
		private readonly string _hint;
		private ImageSource _icon;

		public RunApplicationAction(string path)
		{
			_path = path;
			_text = Path.GetFileNameWithoutExtension(_path);
			_hint = string.Format("Runs {0}", _text);
		}

		public string Text
		{
			get { return _text; }
		}

		public string Hint
		{
			get { return _hint; }
		}

		public ImageSource Icon
		{
			get
			{
				if (_icon == null)
				{
					_icon = NativeMethods.GetShortcutIcon(_path);
				}

				return _icon;
			}
		}

		public Type ResultType
		{
			get { return typeof(ActionResult); }
		}

		public bool IsFallbackMatch { get; set; }

		public ActionResult Execute(ExecutionContext context)
		{
			try
			{
				var startInfo = new ProcessStartInfo(_path);

				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					startInfo.Verb = "runas";
				}

				Process.Start(startInfo);
			}
			catch (Exception)
			{
			}

			return ActionResult.Default;
		}

		public override string ToString()
		{
			return Text;
		}
	}
}