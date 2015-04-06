using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Async
{
	public class AsyncRunner<ResultType, StateType>
	{
		public async void AsyncRun<TrackedResultType>(Func<StateType, TrackedResultType> func, Action<TrackedResultType> callback, StateType state, int trackedId) where TrackedResultType : TrackedResult, ResultType
		{
			TrackedResultType result = await Task<TrackedResultType>.Factory.StartNew(s => func.Invoke((StateType)s), state);
			result.ResultId = trackedId;
			if (callback != null) callback(result);
		}

		public async void AsyncRun<TrackedResultType>(Func<TrackedResultType> func, Action<TrackedResultType> callback, int trackedId) where TrackedResultType : TrackedResult, ResultType
		{
			TrackedResultType result = await Task<TrackedResultType>.Factory.StartNew(func);
			result.ResultId = trackedId;
			if (callback != null) callback(result);
		}

		public async void AsyncRun(Func<StateType, ResultType> func, Action<ResultType> callback, StateType state)
		{
			ResultType result = await Task<ResultType>.Factory.StartNew(s => func.Invoke((StateType)s), state);
			if (callback != null) callback(result);
		}

		public async void AsyncRun(Func<ResultType> func, Action<ResultType> callback)
		{
			ResultType result = await Task<ResultType>.Factory.StartNew(func);
			if (callback != null) callback(result);
		}
	}
}
