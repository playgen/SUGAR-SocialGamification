using System;

namespace PlayGen.SUGAR.Client.AsyncRequestQueue
{
	public interface IAsyncRequestController : IDisposable
	{
		void EnqueueRequest(Action request, Action onSuccess, Action<Exception> onError);

		void EnqueueRequest<TResult>(Func<TResult> request, Action<TResult> onSuccess, Action<Exception> onError);

		bool TryExecuteResponse();

		void Clear();
	}
}