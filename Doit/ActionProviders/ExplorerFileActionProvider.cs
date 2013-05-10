using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Doit.Actions;
using SHDocVw;
using Shell32;

namespace Doit.ActionProviders
{
	public class ExplorerFileActionProvider : IActionProvider<FileAction>
	{
		public ICollection<Type> CanConsume { get; private set; }
		
		public bool IsFallback { get; set; }

		public IEnumerable<FileAction> Offer(string query)
		{
			if (!string.IsNullOrEmpty(query))
			{
				yield break;
			}

			var shellWindows = new ShellWindows();

			object pvarloc = null;
			object pvarlocroot = null;
			int phwnd;

			var desktop = (InternetExplorer)shellWindows.FindWindowSW(ref pvarloc, ref pvarlocroot, 0x00000008, out phwnd, 0x00000001);

			var desktopFiles = GetSelectedFiles(desktop).ToArray();

			if (desktopFiles.Length == 1)
			{
				FileAction fileAction = null;

				try
				{
					fileAction = new FileAction(desktopFiles[0]) { Hint = "On desktop" };
				}
				catch (ArgumentException)
				{
				}

				yield return fileAction;
			}

			if (desktopFiles.Length > 1)
			{
				yield return new FileAction(desktopFiles) { Hint = "On desktop" };
			}

			if (MainWindowViewModel.Instance.LastActiveWindowHandle > 0)
			{
				var activeWindow = shellWindows.OfType<InternetExplorer>().FirstOrDefault(w => w.HWND == MainWindowViewModel.Instance.LastActiveWindowHandle);

				if (activeWindow != null)
				{
					var hint = string.Empty;
					Uri uri;

					if (Uri.TryCreate(activeWindow.LocationURL, UriKind.Absolute, out uri))
					{
						hint = string.Format("At '{0}' folder", new DirectoryInfo(uri.LocalPath).Name);
					}

					var activeWindowFiles = GetSelectedFiles(activeWindow).ToArray();

					if (activeWindowFiles.Length == 1)
					{
						FileAction fileAction = null;

						try
						{
							fileAction = new FileAction(activeWindowFiles[0]) { Hint = hint };
						}
						catch (ArgumentException)
						{
						}

						yield return fileAction;
					}

					if (activeWindowFiles.Length > 1)
					{
						FileAction fileAction = null;

						try
						{
							fileAction = new FileAction(activeWindowFiles) { Hint = hint };
						}
						catch (ArgumentException)
						{
						}

						yield return fileAction;
					}
				}
			}
		}

		public IEnumerable<FileAction> Offer(IAction action, string query)
		{
			return Enumerable.Empty<FileAction>();
		}

		private static IEnumerable<string> GetSelectedFiles(InternetExplorer window)
		{
			if (window == null || !string.Equals(Path.GetFileNameWithoutExtension(window.FullName), "explorer", StringComparison.InvariantCultureIgnoreCase))
			{
				return Enumerable.Empty<string>();
			}
			
			var items = ((IShellFolderViewDual2)window.Document).SelectedItems();

			return items.OfType<FolderItem>().Where(i => i.IsFileSystem).Select(i => i.Path);
		}
	}
}