using System;
using System.Windows.Media;
using Doit.ActionResults;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public interface IAction
	{
		string Text { get; }

		string Hint { get; }

		ImageSource Icon { get; }

		Type ResultType { get; }

		ActionResult Execute(ExecutionContext context);
	}
}