using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
	public class ModelSerializer
	{
		public static Dictionary<string, string> SerializeModelToHash(object model)
		{
			Dictionary<string, string> hash = new Dictionary<string, string>();
			PropertyInfo[] props = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			JsonSerializerSettings set = new JsonSerializerSettings();
			set.TypeNameHandling = TypeNameHandling.All;
			foreach (PropertyInfo prop in props)
			{
				var serializeAttr = prop.GetCustomAttributes(typeof(SerializeAttribute));
				if (serializeAttr.Count() > 0 && ((SerializeAttribute)serializeAttr.First()).Serialize == false)
				{
					// If [Serialize(false)] is present, skip
					continue;
				}
				// Also, properties must have a getter and a setter
				if (prop.CanWrite && prop.CanRead)
				{
					hash.Add(prop.Name, JsonConvert.SerializeObject(prop.GetValue(model), prop.GetType(), set));
				}
			}
			return hash;
		}

		public static object DeserializeModelFromHash(Dictionary<string, string> hash, Type modelType)
		{
			object instance = Activator.CreateInstance(modelType);
			PropertyInfo[] props = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			JsonSerializerSettings set = new JsonSerializerSettings();
			set.TypeNameHandling = TypeNameHandling.All;
			foreach (PropertyInfo prop in props)
			{
				var serializeAttr = prop.GetCustomAttributes(typeof(SerializeAttribute));
				if (serializeAttr.Count() > 0 && ((SerializeAttribute)serializeAttr.First()).Serialize == false)
				{
					// If [Serialize(false)] is present, skip
					continue;
				}

				// Also, properties must have a getter and a setter
				if (prop.CanWrite && prop.CanRead)
				{
					if (hash.ContainsKey(prop.Name))
					{
						object value = JsonConvert.DeserializeObject(hash[prop.Name], set);
						if (value != null && !prop.PropertyType.IsAssignableFrom(value.GetType()))
						{
							value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFrom(value);
						}
						prop.SetValue(instance, value);
					}
					else
					{
						// Set specified default if the hash key is not present
						var defaultValueAttr = prop.GetCustomAttributes(typeof(DefaultValueAttribute));
						if (defaultValueAttr.Count() > 0)
						{
							prop.SetValue(instance, ((DefaultValueAttribute)defaultValueAttr.First()).Value);
						}
					}
				}
			}
			return instance;
		}

	}
}
