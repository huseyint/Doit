using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Doit.ActionResults;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public class GoToAddressAction : IAction
	{
		private readonly Uri _uri;
		private readonly ImageSource _icon;
		private readonly string _hint;

		public GoToAddressAction(Uri uri)
		{
			_uri = uri;

			Text = string.Format("Go to {0}", _uri.AbsoluteUri);
			_icon = new BitmapImage(new Uri("pack://application:,,,/Images/OpenViewInBrowser32.png"));
			_hint = "Goes to web address on browser";
		}

		public string Text { get; set; }

		public string Hint
		{
			get { return _hint; }
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
			Process.Start(_uri.AbsoluteUri);

			return ActionResult.Default;
		}

		public override string ToString()
		{
			return Text;
		}
	}
}