using Common.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.UI.Win32Interop
{
	public sealed class GlobalHotkeyHook : IDisposable
	{
		public string Name { get; set; }
		public string Description { get; set; }

		private KeyCombo _Key;
		public KeyCombo Key
		{
			get
			{
				return _Key;
			}
			set
			{
				_Key = value;
				UnregisterHotKey(_window.Handle, 1);
				if (Enabled && _Key != null) RegisterHotKey(_window.Handle, 1, (uint)_Key.Modifier, (uint)_Key.Key);
			}
		}

		private bool _ManagerEnabled;
		public bool ManagerEnabled
		{
			get
			{
				return _ManagerEnabled;
			}
			set
			{
				_ManagerEnabled = value;
				RegisterOrUnregister();
			}
		}

		private bool _Enabled;
		public bool Enabled
		{
			get
			{
				return _Enabled;
			}
			set
			{
				_Enabled = value;
				RegisterOrUnregister();
			}
		}

		#region Private members

		private Window _window = new Window();

		private void RegisterOrUnregister()
		{
			if (_ManagerEnabled && _Enabled && _Key != null)
			{
				RegisterHotKey(_window.Handle, 1, (uint)_Key.Modifier, (uint)_Key.Key);
			}
			else
			{
				UnregisterHotKey(_window.Handle, 1);
			}
		}

		#endregion

		public GlobalHotkeyHook()
		{
			// register the event of the inner native window.
			_window.KeyPressed += delegate(object sender, KeyEventModArgs args)
			{
				if (KeyPressed != null)
				{
					KeyPressed(this, args);
				}
			};
		}

		#region Internal utility types

		/// <summary>
		/// The enumeration of possible modifiers.
		/// </summary>
		[Flags]
		public enum ModifierKeys : uint
		{
			Alt = 1,
			Control = 2,
			Shift = 4,
			Win = 8
		}

		/// <summary>
		/// Single object type for passing a key combination
		/// </summary>
		public class KeyCombo
		{
			public Keys Key { get; set; }
			public ModifierKeys Modifier { get; set; }

			public override string ToString()
			{
				StringBuilder sb = new StringBuilder();
				if (Modifier.HasFlag(ModifierKeys.Control)) sb.Append(Resources.Ctrl);
				if (Modifier.HasFlag(ModifierKeys.Shift))
				{
					if (sb.Length > 0) sb.Append(" + ");
					sb.Append(Resources.Shift);
				}
				if (Modifier.HasFlag(ModifierKeys.Alt))
				{
					if (sb.Length > 0) sb.Append(" + ");
					sb.Append(Resources.Alt);
				}
				if (Modifier.HasFlag(ModifierKeys.Win))
				{
					if (sb.Length > 0) sb.Append(" + ");
					sb.Append(Resources.Win);
				}
				if (sb.Length > 0) sb.Append(" + ");
				sb.Append(Key);
				return sb.ToString();
			}
		}

		/// <summary>
		/// Represents the window that is used internally to get the messages.
		/// </summary>
		private class Window : NativeWindow, IDisposable
		{
			private static int WM_HOTKEY = 0x0312;

			public Window()
			{
				// create the handle for the window.
				this.CreateHandle(new CreateParams());
			}

			/// <summary>
			/// Overridden to get the notifications.
			/// </summary>
			/// <param name="m"></param>
			protected override void WndProc(ref Message m)
			{
				base.WndProc(ref m);

				// check if we got a hot key pressed.
				if (m.Msg == WM_HOTKEY)
				{
					// get the keys.
					Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
					ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

					// invoke the event to notify the parent.
					if (KeyPressed != null)
					{
						KeyPressed(this, new KeyEventModArgs(key, modifier));
					}
				}
			}

			public event EventHandler<KeyEventModArgs> KeyPressed;

			#region IDisposable Members

			public void Dispose()
			{
				this.DestroyHandle();
			}

			#endregion
		}

		#endregion

		#region KeyPressedEvent

		/// <summary>
		/// A hot key has been pressed.
		/// </summary>
		public event EventHandler<KeyEventModArgs> KeyPressed;

		public class KeyEventModArgs : KeyEventArgs
		{
			public KeyEventModArgs(Keys key, ModifierKeys modifier)
				: base(key)
			{
				ModifierKeys = modifier;
			}

			public ModifierKeys ModifierKeys { get; set; }

			public KeyCombo KeyCombo
			{
				get
				{
					return new KeyCombo { Key = KeyCode, Modifier = ModifierKeys };
				}
			}
		}

		#endregion

		#region IDisposable interface

		public void Dispose()
		{
			// remove the registration
			UnregisterHotKey(_window.Handle, 1);

			// dispose the inner native window.
			_window.Dispose();
		}

		#endregion

		#region Native interop

		// Registers a hot key with Windows.
		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		// Unregisters the hot key with Windows.
		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		[DllImport("kernel32.dll")]
		private static extern uint GetLastError();

		#endregion
	}
}
