using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Async
{
	public class TrackedResult : BasicResult
	{
		public Nullable<int> ResultId { get; set; }
	}
}
