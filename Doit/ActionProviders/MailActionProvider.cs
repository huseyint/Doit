using System;
using System.Collections.Generic;
using System.Linq;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public class MailActionProvider : IActionProvider<MailAction>
	{
		private readonly ICollection<Type> _canConsume = new[] { typeof(FileActionResult), typeof(TextActionResult) };

		public ICollection<Type> CanConsume
		{
			get { return _canConsume; }
		}

		public IEnumerable<MailAction> Offer(string query)
		{
			return Enumerable.Empty<MailAction>();
		}

		public IEnumerable<MailAction> Offer(IAction action)
		{
			yield return new MailAction();
		}
	}
}