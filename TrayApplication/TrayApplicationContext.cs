using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.TrayApplication
{
	public abstract class TrayApplicationContext : ApplicationContext
	{
		#region Public properties

		/// <summary>
		/// Should the options form be shown?
		/// </summary>
		public bool ShowOptionsForm { get; set; }

		/// <summary>
		/// Logging interface
		/// </summary>
		public ILog Logger { get; private set; }

		/// <summary>
		/// Absolute path to the application data folder
		/// </summary>
		public string AppDataFolder { get; protected set; }

		#endregion

		#region Members

		protected OptionsForm optionsForm = null;
		protected NotifyIcon notifyIcon;
		private IContainer components;

		#endregion

		public TrayApplicationContext()
		{
			ShowOptionsForm = true;

			// Init logger
			Logger = LogManager.GetLogger(GetType());
		}

		/// <summary>
		/// Initialize the context, create forms (if needed), create the tray icon
		/// </summary>
		public void InitializeContext()
		{
			components = new System.ComponentModel.Container();
			notifyIcon = new NotifyIcon(components)
			{
				ContextMenuStrip = new ContextMenuStrip(),
				Icon = GetApplicationIcon(),
				Text = GetApplicationName(),
				Visible = true
			};
			notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
			notifyIcon.DoubleClick += notifyIcon_DoubleClick;

			OnInitializeContext();

			if (ShowOptionsForm)
			{
				CreateOptionsForm();
			}
		}

		protected void CreateOptionsForm(string openFeed = null)
		{
			if (optionsForm == null)
			{
				optionsForm = BuildOptionsForm();
				optionsForm.Icon = GetApplicationIcon();
				optionsForm.Closed += optionsForm_Closed;
				optionsForm.Show();
			}
			else
			{
				optionsForm.Activate();
			}
		}

		#region Abstract interface

		protected abstract void OnInitializeContext();

		protected abstract OptionsForm BuildOptionsForm();

		protected abstract void BuildContextMenu();

		protected abstract Icon GetApplicationIcon();

		protected abstract string GetApplicationName();

		protected abstract string GetAppDataPath();

		#endregion

		#region Event handlers

		private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			e.Cancel = false;
			notifyIcon.ContextMenuStrip.Items.Clear();

			BuildContextMenu();
		}

		private void notifyIcon_DoubleClick(object sender, EventArgs e)
		{
			CreateOptionsForm();
		}

		private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
				mi.Invoke(notifyIcon, null);
			}
		}

		// null out the forms so we know to create a new one.
		private void optionsForm_Closed(object sender, EventArgs e)
		{
			optionsForm = null;
		}

		#endregion

		#region Overridden base-class methods

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}

		protected override void ExitThreadCore()
		{
			Logger.Info("Shutting down...");

			// close the options form (if needed)
			if (optionsForm != null)
			{
				optionsForm.Close();
			}

			// remove the tray icon
			if (notifyIcon != null)
			{
				notifyIcon.Visible = false;
			}

			// finish exiting
			base.ExitThreadCore();
		}

		#endregion
	}
}
