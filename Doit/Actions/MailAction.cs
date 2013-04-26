using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public class MailAction : IAction
	{
		private readonly ImageSource _icon;

		private string _text;
		
		private string _hint;

		public MailAction()
		{
			_text = "Send mail";

			_icon = new BitmapImage(new Uri("pack://application:,,,/Images/Mail32.png"));

			_hint = "Composes a mail";
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
			get { return _icon; }
		}

		public ActionResult Execute(ExecutionContext context)
		{
			var args = string.Empty;

			if (context.LastActionResult is FileActionResult)
			{
				var fileActionResult = (FileActionResult)context.LastActionResult;

				args = string.Format("-compose \"attachment='{0}'\"", string.Join(",", fileActionResult.Paths));
			}

			Process.Start(@"C:\Program Files (x86)\Mozilla Thunderbird\thunderbird.exe", args);

			return ActionResult.Default;
		}
	}
}