using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public class SearchWebActionProvider : SingleParameterActionProvider<SearchWebAction>
	{
		private readonly string _name;

		private readonly string _template;

		public SearchWebActionProvider(string name, string template) : base(name)
		{
			_name = name;
			_template = template;
		}

		public ImageSource Icon { get; set; }

		protected override IEnumerable<SearchWebAction> OfferCore(string parameter)
		{
			if (string.IsNullOrEmpty(parameter))
			{
				return Enumerable.Empty<SearchWebAction>();
			}

			return new[] { new SearchWebAction(_template, parameter, _name) { Icon = Icon } };
		}
	}
}