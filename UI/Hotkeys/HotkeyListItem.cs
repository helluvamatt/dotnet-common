using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.UI.Win32Interop;
using Common.Properties;

namespace Common.UI.Hotkeys
{
	public partial class HotkeyListItem : UserControl
	{
		public GlobalHotkeyHook Hook { get; private set; }

		public string ID { get; private set; }

		public HotkeyListItem(string id, GlobalHotkeyHook hook)
		{
			ID = id;
			Hook = hook;
			InitializeComponent();
			groupBox.Text = hook.Name;
			descriptionLabel.Text = hook.Description;
			enabledCheckBox.Checked = Hook.Enabled;
			hotkeyButton.Text = Hook.Key != null ? Hook.Key.ToString() : Resources.NotSet;
			hotkeyButton.Click += hotkeyButton_Click;
			enabledCheckBox.CheckedChanged += enabledCheckBox_CheckedChanged;
		}

		private void hotkeyButton_Click(object sender, EventArgs e)
		{
			HotkeyForm hotkeyForm = new HotkeyForm();
			hotkeyForm.Cancelled += (t, args) => { hotkeyForm.Close(); hotkeyForm.Dispose(); };
			hotkeyForm.KeyComboPressed += hotkeyForm_KeyComboPressed;
			hotkeyForm.Show();
		}

		private void hotkeyForm_KeyComboPressed(object sender, GlobalHotkeyHook.KeyEventModArgs e)
		{
			HotkeyForm form = (HotkeyForm)sender;
			form.Close();
			form.Dispose();
			Hook.Key = e.KeyCombo;
			hotkeyButton.Text = e.KeyCombo != null ? e.KeyCombo.ToString() : Resources.NotSet;
			OnHotkeyChanged();
		}

		private void enabledCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			bool enabled = enabledCheckBox.Checked;
			Hook.Enabled = enabled;
			OnHotkeyChanged();
		}

		private void OnHotkeyChanged()
		{
			if (HotkeyChanged != null)
			{
				HotkeyChanged(this, new HotkeySetComboEventArgs { ItemID = ID, ItemHook = Hook });
			}
		}

		public event EventHandler<HotkeySetComboEventArgs> HotkeyChanged;

		public class HotkeySetComboEventArgs : EventArgs
		{
			public string ItemID { get; set; }
			public GlobalHotkeyHook ItemHook { get; set; }
		}
	}
}
