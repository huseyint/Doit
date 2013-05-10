using System;
using System.Collections.Generic;
using System.Linq;
using Doit.ActionResults;
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

		public bool IsFallback { get; set; }

		public IEnumerable<MailAction> Offer(string query)
		{
			return Enumerable.Empty<MailAction>();
		}

		public IEnumerable<MailAction> Offer(IAction action, string query)
		{
			yield return new MailAction();
		}
	}
}