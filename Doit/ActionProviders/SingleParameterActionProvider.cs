using System;
using System.Collections.Generic;
using System.Linq;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public abstract class SingleParameterActionProvider<T> : IActionProvider<T> where T : IAction
	{
		private readonly string _command;

		protected SingleParameterActionProvider(string command)
		{
			_command = command;
		}

		public virtual ICollection<Type> CanConsume { get; private set; }

		public bool IsFallback { get; set; }

		public virtual IEnumerable<T> Offer(string query)
		{
			if (query != null && query.StartsWith(_command, StringComparison.InvariantCultureIgnoreCase))
			{
				if (_command.Length == query.Length)
				{
					return OfferCore(string.Empty);
				}

				if (_command.Length < query.Length && query[_command.Length] == ' ')
				{
					return OfferCore(query.Substring(_command.Length + 1));
				}
			}

			if (IsFallback)
			{
				return OfferCore(query);
			}

			return Enumerable.Empty<T>();
		}

		public virtual IEnumerable<T> Offer(IAction action, string query)
		{
			return Enumerable.Empty<T>();
		}

		protected abstract IEnumerable<T> OfferCore(string parameter);
	}
}