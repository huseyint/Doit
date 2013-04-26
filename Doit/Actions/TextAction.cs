using System.Windows.Media;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public class TextAction : IAction
	{
		public string Text { get; private set; }
		
		public string Hint { get; private set; }
		
		public ImageSource Icon { get; private set; }

		public ActionResult Execute(ExecutionContext context)
		{
			return ActionResult.Default;
		}
	}
}