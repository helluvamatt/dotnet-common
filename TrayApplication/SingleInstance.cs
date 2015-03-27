using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.TrayApplication
{
	static public class SingleInstance
	{
		//public static readonly int WM_SHOWFIRSTINSTANCE = WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
		static Mutex mutex;
		static public bool Start()
		{
			bool onlyInstance = false;
			string mutexName = String.Format("Local\\{0}", AssemblyGuid);
			mutex = new Mutex(true, mutexName, out onlyInstance);
			return onlyInstance;
		}

		static public void Stop()
		{
			mutex.ReleaseMutex();
		}

		static public string AssemblyGuid
		{
			get
			{
				object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
				if (attributes.Length == 0)
				{
					return String.Empty;
				}
				return ((System.Runtime.InteropServices.GuidAttribute)attributes[0]).Value;
			}
		}
	}
}
