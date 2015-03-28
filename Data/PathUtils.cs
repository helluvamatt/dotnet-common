using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Data
{
	public class PathUtils
	{
		/// <summary>
		/// Create a valid path name from a title
		/// 
		/// From here: 
		/// </summary>
		/// <param name="filename">Source title/filename</param>
		/// <returns>Proper legal filename</returns>
		public static string CoerceValidFileName(string filename)
		{
			string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
			string invalidReStr = string.Format(@"[{0}]+", invalidChars);
			return Regex.Replace(filename, invalidReStr, "_");
		}

		/// <summary>
		/// Create a valid URL from a title
		/// 
		/// Slightly modified from here: http://stackoverflow.com/questions/37809/how-do-i-generate-a-friendly-url-in-c
		/// </summary>
		/// <param name="title">Title to convert</param>
		/// <returns>Valid/legal URL</returns>
		public static string CoerceValidUrl(string title)
		{
			// make it all lower case
			title = title.ToLower();
			// remove entities
			title = Regex.Replace(title, @"&\w+;", "");
			// remove anything that is not letters, numbers, dash, or space
			title = Regex.Replace(title, @"[^a-z0-9\-\s]", "");
			// collapse whitespace to an underscore
			title = Regex.Replace(title, @"\s+", "_");
			// trim excessive dashes at the beginning
			title = title.TrimStart(new[] { '_' });
			// if it's too long, clip it
			if (title.Length > 80)
				title = title.Substring(0, 79);
			// remove trailing dashes
			title = title.TrimEnd(new[] { '_' });
			return title;
		}
	}
}
