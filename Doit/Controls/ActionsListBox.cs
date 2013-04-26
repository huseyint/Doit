using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Doit.Controls
{
	public class ActionsListBox : ListBox
	{
		protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);

			Debug.WriteLine("OnItemsChanged");

			Visibility = Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
		}
	}
}