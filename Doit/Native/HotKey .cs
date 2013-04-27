using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace Doit.Native
{
	public sealed class HotKey : IDisposable
	{
		public const int WmHotKey = 0x0312;

		private readonly int _id;

		private readonly IntPtr _handle;
		
		private bool _isKeyRegistered;

		private bool _disposed;

		public HotKey(ModifierKeys modifierKeys, Keys key, Window window)
			: this(modifierKeys, key, new WindowInteropHelper(window))
		{
			Contract.Requires(window != null);
		}

		public HotKey(ModifierKeys modifierKeys, Keys key, WindowInteropHelper window)
			: this(modifierKeys, key, window.Handle)
		{
			Contract.Requires(window != null);
		}

		public HotKey(ModifierKeys modifierKeys, Keys key, IntPtr windowHandle)
		{
			Contract.Requires(modifierKeys != ModifierKeys.None || key != Keys.None);
			Contract.Requires(windowHandle != IntPtr.Zero);

			Key = key;
			KeyModifier = modifierKeys;
			_id = GetHashCode();
			_handle = windowHandle;
			ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessageMethod;
		}

		~HotKey()
		{
			Dispose(false);
		}

		public event Action<HotKey> HotKeyPressed;

		public Keys Key { get; private set; }

		public ModifierKeys KeyModifier { get; private set; }

		public bool Register()
		{
			if (Key == Keys.None)
			{
				return false;
			}

			if (_isKeyRegistered)
			{
				Unregister();
			}

			return NativeMethods.RegisterHotKey(_handle, _id, KeyModifier, Key);
		}

		public void Unregister()
		{
			_isKeyRegistered = !NativeMethods.UnregisterHotKey(_handle, _id);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					ComponentDispatcher.ThreadPreprocessMessage -= ThreadPreprocessMessageMethod;
				}

				Unregister();
				_disposed = true;
			}
		}

		private void ThreadPreprocessMessageMethod(ref MSG msg, ref bool handled)
		{
			if (!handled)
			{
				if (msg.message == WmHotKey && (int)msg.wParam == _id)
				{
					OnHotKeyPressed();
					handled = true;
				}
			}
		}

		private void OnHotKeyPressed()
		{
			if (HotKeyPressed != null)
			{
				HotKeyPressed(this);
			}
		}
	}
}