using System;

namespace Doit.Infrastructure
{
	public class ChangeVisibilityEventArgs : EventArgs
	{
		public bool IsVisible { get; set; }
	}
}