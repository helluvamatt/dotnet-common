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
	public sealed class LocalizedCategoryAttribute : CategoryAttribute
	{
		private ResourceManager _ResourceManager;

		public LocalizedCategoryAttribute(Type resourceManagerProvider, string resourceId)
			: base(resourceId)
		{
			foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic))
			{
				if (staticProperty.PropertyType == typeof(ResourceManager))
				{
					_ResourceManager = (ResourceManager)staticProperty.GetValue(null, null);
				}
			}
		}

		protected override string GetLocalizedString(string value)
		{
			return _ResourceManager.GetString(value);
		}
	}
}
