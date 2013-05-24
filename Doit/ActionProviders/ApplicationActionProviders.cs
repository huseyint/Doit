using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Doit.Actions;
using Doit.Infrastructure;

namespace Doit.ActionProviders
{
	public class ApplicationActionProviders : IActionProvider<DelegateAction>
	{
		private readonly Dictionary<string, DelegateAction> _actions;

		public ApplicationActionProviders()
		{
			_actions = new Dictionary<string, DelegateAction>(StringComparer.InvariantCultureIgnoreCase)
			{
				{ "quit", new DelegateAction(Quit) { Text = "Quit", Icon = Utils.GetFreezedImage("Quit32.png"), Hint = "Bye!" } },
			};
		}

		public ICollection<Type> CanConsume { get; private set; }

		public bool IsFallback { get; set; }

		public IEnumerable<DelegateAction> Offer(string query)
		{
			DelegateAction delegateAction;

			if (_actions.TryGetValue(query, out delegateAction))
			{
				return new[] { delegateAction };
			}

			return Enumerable.Empty<DelegateAction>();
		}

		public IEnumerable<DelegateAction> Offer(IAction action, string query)
		{
			throw new NotImplementedException();
		}

		private static void Quit()
		{
			Application.Current.Shutdown(0);
		}
	}
}