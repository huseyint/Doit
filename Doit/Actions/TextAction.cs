using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Doit.ActionResults;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public class TextAction : IAction
	{
		private const int MaxDisplayTextLength = 25;

		private static readonly BitmapSource _icon;

		private readonly string _text;
		
		private readonly string _displayText;

		static TextAction()
		{
			_icon = new BitmapImage(new Uri("pack://application:,,,/Images/Text32.png"));
		}

		public TextAction(string text)
		{
			_text = text ?? string.Empty;

			var reader = new StringReader(_text);

			string line;

			while ((line = reader.ReadLine()) != null)
			{
				if (!string.IsNullOrWhiteSpace(line))
				{
					break;
				}
			}

			if (line != null)
			{
				_displayText = line.Length > MaxDisplayTextLength ? line.Substring(0, MaxDisplayTextLength) + "..." : line;

				_displayText = _displayText.Replace("\t", string.Empty);
			}
		}

		public string Text
		{
			get { return _displayText; }
		}

		public string Hint
		{
			get { return "Text on clipboard"; }
		}

		public ImageSource Icon
		{
			get { return _icon; }
		}

		public Type ResultType
		{
			get { return typeof(TextActionResult); }
		}

		public ActionResult Execute(ExecutionContext context)
		{
			if (context.HasNextAction)
			{
				return new TextActionResult { Text = _text };
			}

			return ActionResult.Default;
		}

		public bool IsFallbackMatch { get; set; }

		public override string ToString()
		{
			return "Text";
		}
	}
}