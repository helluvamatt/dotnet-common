using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Async
{
	public class AsyncRunner<ResultType, StateType>
	{
		public async void AsyncRun<TrackedResultType>(Func<StateType, TrackedResultType> func, AsyncCallback<TrackedResultType> callback, StateType state, int trackedId) where TrackedResultType : TrackedResult, ResultType
		{
			TrackedResultType result = await Task<TrackedResultType>.Factory.StartNew(s => func.Invoke((StateType)s), state);
			result.ResultId = trackedId;
			if (callback != null) callback.Finished(result);
		}

		public async void AsyncRun<TrackedResultType>(Func<TrackedResultType> func, AsyncCallback<TrackedResultType> callback, int trackedId) where TrackedResultType : TrackedResult, ResultType
		{
			TrackedResultType result = await Task<TrackedResultType>.Factory.StartNew(func);
			result.ResultId = trackedId;
			if (callback != null) callback.Finished(result);
		}

		public async void AsyncRun(Func<StateType,ResultType> func, AsyncCallback<ResultType> callback, StateType state)
		{
			ResultType result = await Task<ResultType>.Factory.StartNew(s => func.Invoke((StateType)s), state);
			if (callback != null) callback.Finished(result);
		}

		public async void AsyncRun(Func<ResultType> func, AsyncCallback<ResultType> callback)
		{
			ResultType result = await Task<ResultType>.Factory.StartNew(func);
			if (callback != null) callback.Finished(result);
		}
	}

	public sealed class AsyncCallback<ResultType>
	{
		public void Finished(ResultType result)
		{
			if (AsyncFinished != null) AsyncFinished(result);
		}

		public event AsyncFinishedEvent AsyncFinished;

		public delegate void AsyncFinishedEvent(ResultType result);
	}
}
