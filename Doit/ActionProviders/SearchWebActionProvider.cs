using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public class SearchWebActionProvider : IActionProvider<SearchWebAction>
	{
		private readonly string _name;
		private readonly string _template;

		public SearchWebActionProvider(string name, string template)
		{
			_name = name;
			_template = template;
		}

		public ImageSource Icon { get; set; }

		public ICollection<Type> CanConsume { get; private set; }

		public IEnumerable<SearchWebAction> Offer(string query)
		{
			if (query != null)
			{
				var command = _name.ToLowerInvariant() + " ";

				if (query.StartsWith(command))
				{
					var text = query.Substring(command.Length);

					return new[] { new SearchWebAction(_template, text, _name) { Icon = Icon } };
				}
			}

			return Enumerable.Empty<SearchWebAction>();
		}

		public IEnumerable<SearchWebAction> Offer(IAction action)
		{
			throw new NotImplementedException();
		}
	}
}