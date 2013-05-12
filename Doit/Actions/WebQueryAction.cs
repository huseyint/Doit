using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Media;
using Doit.ActionResults;
using Doit.Infrastructure;

namespace Doit.Actions
{
	public class WebQueryAction : IAction
	{
		private readonly string _address;
		private readonly string _query;
		private string _hint;

		public WebQueryAction(string address, string query, string name)
		{
			_address = address;
			_query = query;

			Text = string.Format("{0} {1}", name, query);
			_hint = string.Format("Searches web for {0}", _query);
		}

		public string Text { get; set; }
		
		public string Hint
		{
			get { return _hint; }
		}

		public ImageSource Icon { get; set; }

		public Type ResultType
		{
			get { return typeof(ActionResult); }
		}

		public ActionResult Execute(ExecutionContext context)
		{
			var address = string.Format(_address, WebUtility.UrlEncode(_query));

			Process.Start(address);

			return ActionResult.Default;
		}

		public bool IsFallbackMatch { get; set; }

		public override string ToString()
		{
			return string.Format("Search {0}", _query);
		}
	}
}