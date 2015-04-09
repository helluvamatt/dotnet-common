using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Data
{
	public sealed class SettingsManager<T> where T : new()
	{
		public T SettingsObject { get; private set; }

		public SettingsManager()
		{
			SettingsObject = new T();
		}

		public Dictionary<string, string> SerializeToHash()
		{
			return ModelSerializer.SerializeModelToHash(SettingsObject);
		}

		public void DeserializeFromHash(Dictionary<string, string> hash)
		{
			SettingsObject = (T) ModelSerializer.DeserializeModelFromHash(hash, typeof(T));
		}

		#region INI file processing

		private static Regex _comment = new Regex("^\\s*;");

		public void SerializeToIni(TextWriter writer)
		{
			Dictionary<string, string> hash = SerializeToHash();
			foreach (KeyValuePair<string, string> entry in hash)
			{
				writer.WriteLine(string.Format("{0}={1}", entry.Key, entry.Value));
			}
			writer.Flush();
		}

		public void DeserializeFromIni(TextReader reader)
		{
			Dictionary<string, string> hash = new Dictionary<string, string>();
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				// Ignore comments
				if (_comment.IsMatch(line))
				{
					continue;
				}

				// Parse valid lines
				int equalsInd = line.IndexOf('=');
				if (equalsInd > 0) // Not a typo: key part must be at least character
				{
					string key = line.Substring(0, equalsInd);
					string value = line.Substring(equalsInd + 1);
					hash[key] = value;
				}
			}
			DeserializeFromHash(hash);
		}

		#endregion

	}
}
