using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public class RunApplicationActionProvider : IActionProvider<RunApplicationAction>
	{
		private readonly RunApplicationAction[] _actions;

		public RunApplicationActionProvider()
		{
			var searchFolders = new[]
			{
				Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu),
				Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
				////KnownFolders.GetKnownFolderPath(KnownFolders.QuickLaunch),
			};

			_actions = searchFolders
				.SelectMany(f => Directory.GetFiles(f, "*.lnk", SearchOption.AllDirectories))
				.Where(f => !f.ToLowerInvariant().Contains("uninstall"))
				.Select(f => new RunApplicationAction(f))
				.OrderBy(a => a.Text)
				.ToArray();
		}

		public ICollection<Type> CanConsume { get; private set; }

		public IEnumerable<RunApplicationAction> Offer(string query)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return Enumerable.Empty<RunApplicationAction>();
			}

			var queryLower = query.ToLowerInvariant();

			return _actions.Where(a => a.Text.ToLowerInvariant().Contains(queryLower));
		}

		public IEnumerable<RunApplicationAction> Offer(IAction action)
		{
			throw new NotImplementedException();
		}
	}
}