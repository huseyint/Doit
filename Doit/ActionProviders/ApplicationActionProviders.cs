using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public class ApplicationActionProviders : IActionProvider<DelegateAction>
	{
		private readonly Dictionary<string, DelegateAction> _actions;

		public ApplicationActionProviders()
		{
			_actions = new Dictionary<string, DelegateAction>(StringComparer.InvariantCultureIgnoreCase)
			{
				{ "quit", new DelegateAction(Quit) { Text = "Quit", Icon = new BitmapImage(new Uri("pack://application:,,,/Images/Quit32.png")), Hint = "Bye!" } },
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

		public IEnumerable<DelegateAction> Offer(IAction action)
		{
			throw new NotImplementedException();
		}

		private static void Quit()
		{
			Application.Current.Shutdown(0);
		}
	}
}