using System;

namespace PlayGen.SUGAR.Client.AsyncRequestQueue
{
	public class SyncRequestController : IAsyncRequestController
	{
		private Exception _workerException;
		private bool _isDisposed;

		~SyncRequestController()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (_isDisposed) return;

			_isDisposed = true;
		}

		public void EnqueueRequest(Action request, Action onSuccess, Action<Exception> onError)
		{
			try
			{
				request.Invoke();
				onSuccess.Invoke();
			}
			catch (Exception ex)
			{
				onError.Invoke(ex);
			}
		}

		public void EnqueueRequest<TResult>(Func<TResult> request, Action<TResult> onSuccess, Action<Exception> onError)
		{
			try
			{
				var result = request.Invoke();
				onSuccess.Invoke(result);
			}
			catch (Exception ex)
			{
				onError.Invoke(ex);
			}
		}

		public void Clear()
		{
			
		}

		public bool TryExecuteResponse()
		{
			return false;
		}
	}
}
