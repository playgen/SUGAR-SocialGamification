using System;

namespace PlayGen.SUGAR.Client.RequestQueue
{
    public interface IRequestQueue
	{
		void EnqueueRequest(
			Action request,
			Action onSuccess,
			Action<Exception> onError);

		void EnqueueRequest<TResult>(
			Func<TResult> request,
			Action<TResult> onSuccess,
			Action<Exception> onError);

		void Clear();

		bool TryExecuteResponse();
	}
}
