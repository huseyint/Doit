using System;
using System.Windows.Media;
using Doit.ActionResults;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public class DelegateAction : IAction
	{
		private readonly Action _action;

		public DelegateAction(Action action)
		{
			_action = action;
		}

		public string Text { get; set; }
		
		public string Hint { get; set; }

		public Type ResultType
		{
			get { return typeof(ActionResult); }
		}

		public ImageSource Icon { get; set; }

		public ActionResult Execute(ExecutionContext context)
		{
			if (_action != null)
			{
				_action();
			}

			return ActionResult.Default;
		}

		public bool IsFallbackMatch { get; set; }
	}
}