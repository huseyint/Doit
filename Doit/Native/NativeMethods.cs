using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Doit.Native
{
	internal static class NativeMethods
	{
		private static Guid CLSID_IShellItem = new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe");

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
		public static extern void SHCreateItemFromParsingName(
			[In][MarshalAs(UnmanagedType.LPWStr)] string pszPath,
			[In] IntPtr pbc,
			[In][MarshalAs(UnmanagedType.LPStruct)] Guid riid,
			[Out][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] out object ppv);

		[DllImport("shell32.dll")]
		public static extern IntPtr ExtractAssociatedIcon(IntPtr hInst, StringBuilder lpIconPath, out ushort lpiIcon);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool DestroyIcon(IntPtr hIcon);

		[DllImport("user32.dll")]
		public static extern int GetForegroundWindow();

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys fsModifiers, Keys vk);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		public static ImageSource GetShortcutIcon(string path)
		{
			object ppsi = null;
			IntPtr hbitmap = IntPtr.Zero;
			BitmapSource source = null;

			try
			{
				SHCreateItemFromParsingName(path, IntPtr.Zero, CLSID_IShellItem, out ppsi);
				((IShellItemImageFactory)ppsi).GetImage(new SIZE(32, 32), SIIGBF.SIIGBF_RESIZETOFIT | SIIGBF.SIIGBF_ICONONLY, out hbitmap);

				// Convert from GDI HBITMAP to WPF BitmapSource.
				source = Imaging.CreateBitmapSourceFromHBitmap(
					hbitmap,
					IntPtr.Zero,
					Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());
			}
			catch (Exception)
			{
			}
			finally
			{
				if (ppsi != null)
				{
					Marshal.ReleaseComObject(ppsi);
				}

				if (hbitmap != IntPtr.Zero)
				{
					Marshal.Release(hbitmap);
				}
			}

			return source;
		}
	}
}