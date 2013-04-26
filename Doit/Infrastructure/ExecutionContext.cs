using Doit.Actions;

namespace Doit.Infrastructure
{
	public class ExecutionContext
	{
		public ActionResult LastActionResult { get; set; }

		public bool HasNextAction { get; set; }
	}
}