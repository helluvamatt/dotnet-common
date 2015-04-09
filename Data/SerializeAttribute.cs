using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class SerializeAttribute : Attribute
	{
		public SerializeAttribute(bool serialize)
		{
			Serialize = serialize;
		}

		public bool Serialize { get; private set; }
	}
}
