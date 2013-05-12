using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public class WebQueryActionProvider : SingleParameterActionProvider<WebQueryAction>
	{
		private readonly string _name;

		private readonly string _template;

		public WebQueryActionProvider(string name, string template) : base(name)
		{
			_name = name;
			_template = template;
		}

		public ImageSource Icon { get; set; }

		protected override IEnumerable<WebQueryAction> OfferCore(string parameter)
		{
			if (string.IsNullOrEmpty(parameter))
			{
				return Enumerable.Empty<WebQueryAction>();
			}

			return new[] { new WebQueryAction(_template, parameter, _name) { Icon = Icon } };
		}
	}
}