using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Doit.ActionResults;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public class CalculationAction : IAction
	{
		private static readonly BitmapSource _icon;

		private readonly double _value;

		static CalculationAction()
		{
			_icon = new BitmapImage(new Uri("pack://application:,,,/Images/Calculate32.png"));
		}

		public CalculationAction(double value)
		{
			_value = value;
		}

		public string Text
		{
			get { return _value.ToString(); }
		}

		public string Hint { get; set; }

		public ImageSource Icon
		{
			get { return _icon; }
		}
		
		public Type ResultType
		{
			get { return typeof(NumberResult); }
		}

		public bool IsFallbackMatch { get; set; }

		public double Value
		{
			get { return _value; }
		}

		public ActionResult Execute(ExecutionContext context)
		{
			return new NumberResult { Value = _value };
		}

		public override string ToString()
		{
			return Text;
		}
	}
}