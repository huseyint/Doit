using System.Windows.Media;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public interface IAction
	{
		string Text { get; }

		string Hint { get; }

		ImageSource Icon { get; }

		ActionResult Execute(ExecutionContext context);
	}
}