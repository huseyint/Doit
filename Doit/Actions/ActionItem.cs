namespace Doit.Actions
{
	public class ActionItem
	{
		private readonly IAction _action;
		private readonly int _order;

		public ActionItem(IAction action, int order)
		{
			_action = action;
			_order = order;
		}

		public IAction Action
		{
			get { return _action; }
		}

		public int Order
		{
			get { return _order; }
		}
	}
}