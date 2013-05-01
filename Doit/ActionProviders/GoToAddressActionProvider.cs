using System;
using System.Collections.Generic;
using System.Linq;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public class GoToAddressActionProvider : SingleParameterActionProvider<GoToAddressAction>
	{
		public GoToAddressActionProvider() : base("go")
		{
		}

		protected override IEnumerable<GoToAddressAction> OfferCore(string parameter)
		{
			if (!parameter.StartsWith("http"))
			{
				parameter = "http://" + parameter;
			}

			Uri uri;
			if (Uri.TryCreate(parameter, UriKind.Absolute, out uri))
			{
				return new[] { new GoToAddressAction(uri) };
			}

			return Enumerable.Empty<GoToAddressAction>();
		}
	}
}