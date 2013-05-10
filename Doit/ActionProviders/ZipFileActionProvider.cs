using System;
using System.Collections.Generic;
using System.Linq;
using Doit.ActionResults;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public class ZipFileActionProvider : IActionProvider<ZipFileAction>
	{
		private readonly ICollection<Type> _canConsume = new[] { typeof(FileActionResult) };

		public ICollection<Type> CanConsume
		{
			get { return _canConsume; }
		}

		public bool IsFallback { get; set; }

		public IEnumerable<ZipFileAction> Offer(string query)
		{
			return Enumerable.Empty<ZipFileAction>();
		}

		public IEnumerable<ZipFileAction> Offer(IAction action, string query)
		{
			yield return new ZipFileAction();
		}
	}
}