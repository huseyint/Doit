using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Doit.ActionResults;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public class MailAction : IAction
	{
		private static readonly ImageSource _icon;

		static MailAction()
		{
			_icon = new BitmapImage(new Uri("pack://application:,,,/Images/Mail32.png"));
		}

		public string Text
		{
			get { return "Send mail"; }
		}

		public string Hint
		{
			get { return "Composes a mail"; }
		}

		public ImageSource Icon
		{
			get { return _icon; }
		}

		public Type ResultType
		{
			get { return typeof(ActionResult); }
		}

		public bool IsFallbackMatch { get; set; }

		public ActionResult Execute(ExecutionContext context)
		{
			// http://kb.mozillazine.org/Command_line_arguments_(Thunderbird)
			var args = string.Empty;

			if (context.LastActionResult is FileActionResult)
			{
				var fileActionResult = (FileActionResult)context.LastActionResult;

				args = string.Format("-compose \"attachment='{0}'\"", string.Join(",", fileActionResult.Paths));
			}
			else if (context.LastActionResult is TextActionResult)
			{
				var textActionResult = (TextActionResult)context.LastActionResult;

				args = string.Format("-compose \"body='{0}'\"", textActionResult.Text);
			}

			Process.Start(@"C:\Program Files (x86)\Mozilla Thunderbird\thunderbird.exe", args);

			return ActionResult.Default;
		}
	}
}