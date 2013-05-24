using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Doit.Actions;
using Doit.Infrastructure;

namespace Doit.ActionProviders
{
	public class ClipboardTextActionProvider : IActionProvider<TextAction>
	{
		public ICollection<Type> CanConsume { get; private set; }
		
		public bool IsFallback { get; set; }

		public IEnumerable<TextAction> Offer(string query)
		{
			if (!string.IsNullOrEmpty(query))
			{
				yield break;
			}

			string text = null;

			Utils.RunOnStaThread(() => text = Clipboard.GetText());

			if (!string.IsNullOrEmpty(text))
			{
				yield return new TextAction(text);
			}
		}

		public IEnumerable<TextAction> Offer(IAction action, string query)
		{
			return Enumerable.Empty<TextAction>();
		}
	}
}