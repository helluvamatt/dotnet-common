using Common.Properties;
using Common.UI.Win32Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.UI.Hotkeys
{
	public partial class HotkeyForm : Form, IMessageFilter
	{
		private const int WM_KEYDOWN = 0x0100;
		private const int WM_KEYUP = 0x0101;

		private bool _WinPressed = false;
		private bool _ShiftPressed = false;
		private bool _CtrlPressed = false;
		private bool _AltPressed = false;
		private Keys _KeyPressed = Keys.None;

		public HotkeyForm()
		{
			InitializeComponent();
			UpdateUI();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Application.AddMessageFilter(this);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			Application.RemoveMessageFilter(this);
		}

		private void UpdateUI()
		{
			StringBuilder sb = new StringBuilder();
			if (_CtrlPressed) sb.Append(Resources.Ctrl);
			if (_ShiftPressed)
			{
				if (sb.Length > 0) sb.Append(" + ");
				sb.Append(Resources.Shift);
			}
			if (_AltPressed)
			{
				if (sb.Length > 0) sb.Append(" + ");
				sb.Append(Resources.Alt);
			}
			if (_WinPressed)
			{
				if (sb.Length > 0) sb.Append(" + ");
				sb.Append(Resources.Win);
			}
			if (_KeyPressed != Keys.None)
			{
				if (sb.Length > 0) sb.Append(" + ");
				sb.Append(_KeyPressed);
			}
			if (sb.Length > 0)
			{
				keyCombinationLabel.Text = sb.ToString();
			}
			else
			{
				keyCombinationLabel.Text = Resources.PressKeyCombination;
			}
		}

		#region IMessageFilter impl

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == WM_KEYDOWN)
			{
				Keys key = (Keys)m.WParam;
				Keys keyCode = key & Keys.KeyCode;
				switch (keyCode)
				{
					case Keys.ShiftKey:
					case Keys.LShiftKey:
					case Keys.RShiftKey:
						_ShiftPressed = true;
						break;
					case Keys.ControlKey:
					case Keys.LControlKey:
					case Keys.RControlKey:
						_CtrlPressed = true;
						break;
					case Keys.LMenu:
					case Keys.RMenu:
					case Keys.Menu:
						_AltPressed = true;
						break;
					case Keys.RWin:
					case Keys.LWin:
						_WinPressed = true;
						break;
					default:
						_KeyPressed = keyCode;
						break;
				}
				UpdateUI();
			}
			else if (m.Msg == WM_KEYUP)
			{
				Keys key = (Keys)m.WParam;
				Keys keyCode = key & Keys.KeyCode;
				switch (keyCode)
				{
					case Keys.ShiftKey:
					case Keys.LShiftKey:
					case Keys.RShiftKey:
						if (_KeyPressed == Keys.None) _ShiftPressed = false;
						break;
					case Keys.ControlKey:
					case Keys.LControlKey:
					case Keys.RControlKey:
						if (_KeyPressed == Keys.None) _CtrlPressed = false;
						break;
					case Keys.LMenu:
					case Keys.RMenu:
					case Keys.Menu:
						if (_KeyPressed == Keys.None) _AltPressed = false;
						break;
					case Keys.RWin:
					case Keys.LWin:
						if (_KeyPressed == Keys.None) _WinPressed = false;
						break;
					default:
						if (keyCode == Keys.Escape && !_ShiftPressed && !_CtrlPressed && !_AltPressed && !_WinPressed)
						{
							if (Cancelled != null) Cancelled(this, new EventArgs());
						}
						else
						{
							if (KeyComboPressed != null)
							{
								GlobalHotkeyHook.ModifierKeys modifier = new GlobalHotkeyHook.ModifierKeys();
								if (_AltPressed) modifier |= GlobalHotkeyHook.ModifierKeys.Alt;
								if (_CtrlPressed) modifier |= GlobalHotkeyHook.ModifierKeys.Control;
								if (_ShiftPressed) modifier |= GlobalHotkeyHook.ModifierKeys.Shift;
								if (_WinPressed) modifier |= GlobalHotkeyHook.ModifierKeys.Win;
								KeyComboPressed(this, new GlobalHotkeyHook.KeyEventModArgs(_KeyPressed, modifier));
							}
						}
						break;
				}
				UpdateUI();
			}

			return false;
		}

		#endregion

		#region Events

		public event EventHandler<GlobalHotkeyHook.KeyEventModArgs> KeyComboPressed;

		public event EventHandler Cancelled;

		#endregion

	}

}
