using System.Collections.Generic;
using System.IO;
using System.Linq;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public class RunApplicationActionProvider : SingleParameterActionProvider<RunApplicationAction>
	{
		private readonly RunApplicationAction[] _actions;

		public RunApplicationActionProvider(ICollection<string> searchFolders)
			: base("run")
		{
			_actions = searchFolders
				.SelectMany(f => Directory.GetFiles(f, "*.lnk", SearchOption.AllDirectories))
				.Where(f => !f.ToLowerInvariant().Contains("uninstall"))
				.Select(f => new RunApplicationAction(f))
				.OrderBy(a => a.Text)
				.ToArray();
		}

		protected override IEnumerable<RunApplicationAction> OfferCore(string parameter)
		{
			if (string.IsNullOrWhiteSpace(parameter))
			{
				return Enumerable.Empty<RunApplicationAction>();
			}

			return _actions.Where(a => a.Text.ToLowerInvariant().Contains(parameter.ToLowerInvariant()));
		}
	}
}