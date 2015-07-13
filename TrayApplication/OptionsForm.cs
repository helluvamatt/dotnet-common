using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.TrayApplication
{
	public class OptionsForm : Form
	{
		#region OptionChanged event

		public delegate void OptionChangedEvent(object sender, OptionChangedEventArgs args);

		public event OptionChangedEvent OptionChanged;

		protected void OnOptionChanged(string name, object value)
		{
			if (OptionChanged != null)
			{
				OptionChanged(this, new OptionChangedEventArgs { Name = name, Value = value });
			}
		}

		#endregion
		
		public virtual void PopulateSettings()
		{
			// Default does nothing
		}
	}

	public class OptionChangedEventArgs : EventArgs
	{
		public string Name { get; set; }
		public object Value { get; set; }
	}
}
