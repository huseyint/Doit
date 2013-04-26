using System;
using System.Collections.Generic;
using Doit.Actions;

namespace Doit.ActionProviders
{
	public interface IActionProvider<out T> where T : IAction
	{
		ICollection<Type> CanConsume { get; }

		IEnumerable<T> Offer(string query);

		IEnumerable<T> Offer(IAction action);
	}
}