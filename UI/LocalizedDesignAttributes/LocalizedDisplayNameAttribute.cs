using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Common.UI.LocalizedDesignAttributes
{
	public sealed class LocalizedDisplayNameAttribute : DisplayNameAttribute
	{
		private CultureInfo cachedCulture;
		private string cachedDisplayName;
		private ResourceManager _ResourceManager;

		public LocalizedDisplayNameAttribute(Type resourceManagerProvider, string resourceId) : base(resourceId)
		{
			foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic))
			{
				if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
				{
					_ResourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
				}
			}
		}

		public override string DisplayName
		{
			get
			{
				var culture = CultureInfo.CurrentCulture;
				if (culture != cachedCulture)
				{
					cachedDisplayName = _ResourceManager.GetString(base.DisplayName);
					cachedCulture = culture;
				}
				if (cachedDisplayName == null) throw new NullReferenceException("Should not be null!");
				return cachedDisplayName;
			}
		}
	}
}
