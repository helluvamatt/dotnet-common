using Common.Data;
using Common.Data.Async;
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
	public abstract class TrayApplicationContext<T> : ApplicationContext where T : new()
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
		/// 
		/// </summary>
		protected SettingsManager<T> SettingsManager { get; private set; }

		#endregion

		#region Members

		protected OptionsForm optionsForm = null;
		private NotifyIcon notifyIcon;
		private IContainer components;

		#endregion

		public TrayApplicationContext()
		{
			ShowOptionsForm = true;

			// Init logger
			Logger = LogManager.GetLogger(GetType());

			// Init SettingsManager
			SettingsManager = new SettingsManager<T>();
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
				Icon = ApplicationIcon,
				Text = ApplicationName,
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

		protected void CreateOptionsForm()
		{
			if (optionsForm == null)
			{
				optionsForm = OnBuildOptionsForm();
				optionsForm.Icon = ApplicationIcon;
				optionsForm.Closed += optionsForm_Closed;
				optionsForm.Show();
			}
			else
			{
				optionsForm.Activate();
			}
		}

		#region Settings loading and saving helpers

		protected void LoadSettingsAsync(Action<T> callback, string iniFile)
		{
			new AsyncRunner<T, String>().AsyncRun(LoadSettings, callback, iniFile);
		}

		protected void SaveSettingsAsync(Action<bool> callback, string iniFile)
		{
			new AsyncRunner<bool, String>().AsyncRun(f => SaveSettings(f), callback, iniFile);
		}

		protected T LoadSettings(string iniFile)
		{
			if (File.Exists(iniFile))
			{
				try
				{
					// Load application configuration from INI file
					using (StreamReader reader = new StreamReader(iniFile, Encoding.UTF8))
					{
						// Populate the settings that were loaded and ensure defaults are set
						SettingsManager.DeserializeFromIni(reader);
					}
				}
				catch (IOException ex)
				{
					// Ensure defaults are set even if we fail to load the file
					Logger.Error("Failed to load settings from INI file.", ex);
					SettingsManager.DeserializeFromHash(new Dictionary<string, string>());
				}
			}
			else
			{
				// The INI file does not exist, load defaults and save them
				Logger.WarnFormat("INI file not found at '{0}'. Creating new one...", iniFile);
				SettingsManager.DeserializeFromHash(new Dictionary<string, string>());
				SaveSettings(iniFile);
			}
			return SettingsManager.SettingsObject;
		}

		protected bool SaveSettings(string iniFile)
		{
			// Save application configuration to INI file
			try
			{
				using (StreamWriter writer = new StreamWriter(iniFile, false, Encoding.UTF8))
				{
					SettingsManager.SerializeToIni(writer);
					writer.Flush();
				}
				return true;
			}
			catch (IOException ex)
			{
				Logger.Error("Failed to save settings to INI file.", ex);
				return false;
			}
		}

		#endregion

		#region Abstract interface

		/// <summary>
		/// Called at some point during application startup. Initial configuration should be started here.
		/// </summary>
		protected abstract void OnInitializeContext();

		/// <summary>
		/// Called when it is necessary to build the options form. This is called whenever the options form needs to be built after is has been closed or the appliation has just started.
		/// </summary>
		/// <returns>Some form that extends OptionsForm</returns>
		protected abstract OptionsForm OnBuildOptionsForm();

		/// <summary>
		/// Called when it is necessary to build the context menu. This is actually called each time the menu is opened.
		/// </summary>
		/// <param name="menu">ContextMenuStrip from the tray icon</param>
		protected abstract void OnBuildContextMenu(ContextMenuStrip menu);

		/// <summary>
		/// Override in subclasses to define the appliction icon
		/// </summary>
		protected abstract Icon ApplicationIcon { get; }

		/// <summary>
		/// Override in subclasses to define the application name
		/// </summary>
		protected abstract string ApplicationName { get; }

		/// <summary>
		/// Override in subclasses to define the application data path
		/// </summary>
		protected abstract string AppDataPath { get; }

		#endregion

		#region Event handlers

		private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			e.Cancel = false;
			notifyIcon.ContextMenuStrip.Items.Clear();

			OnBuildContextMenu(notifyIcon.ContextMenuStrip);
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
