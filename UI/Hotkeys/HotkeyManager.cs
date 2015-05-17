using Common.UI.Win32Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.UI.Hotkeys
{
	public class HotkeyManager : IDisposable
	{
		#region Private members

		private Dictionary<string, GlobalHotkeyHook> _Hooks = new Dictionary<string, GlobalHotkeyHook>();

		#endregion

		public void AddHook(string id, string name, string description, Action callback)
		{
			if (id == null) throw new ArgumentNullException("id");
			if (callback == null) throw new ArgumentNullException("callback");
			var hook = new GlobalHotkeyHook();
			hook.Name = name;
			hook.Description = description;
			hook.KeyPressed += (t, args) => { callback(); };
			_Hooks.Add(id, hook);
		}

		public GlobalHotkeyHook GetHook(string id)
		{
			if (!_Hooks.ContainsKey(id)) throw new ArgumentException("Invalid hook with id = " + id);
			return _Hooks[id];
		}

		public IEnumerable<KeyValuePair<string, GlobalHotkeyHook>> GetHooks()
		{
			return _Hooks;
		}

		#region Properties

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
				foreach (GlobalHotkeyHook hook in _Hooks.Values)
				{
					hook.ManagerEnabled = _Enabled;
				}
			}
		}

		#endregion

		#region IDisposable implementation

		public void Dispose()
		{
			foreach (var hook in _Hooks.Values)
			{
				hook.Dispose();
			}
		}

		#endregion
	}
}
