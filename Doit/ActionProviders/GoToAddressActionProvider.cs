using System;
using System.Collections.Generic;
using System.Linq;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public class GoToAddressActionProvider : IActionProvider<GoToAddressAction>
	{
		public ICollection<Type> CanConsume { get; private set; }

		public IEnumerable<GoToAddressAction> Offer(string query)
		{
			if (query != null && query.StartsWith("go ", StringComparison.InvariantCultureIgnoreCase))
			{
				var address = query.Substring(3);

				if (!address.StartsWith("http"))
				{
					address = "http://" + address;
				}

				Uri uri;
				if (Uri.TryCreate(address, UriKind.Absolute, out uri))
				{
					return new[] { new GoToAddressAction(uri) };
				}
			}

			return Enumerable.Empty<GoToAddressAction>();
		}

		public IEnumerable<GoToAddressAction> Offer(IAction action)
		{
			throw new NotImplementedException();
		}
	}
}