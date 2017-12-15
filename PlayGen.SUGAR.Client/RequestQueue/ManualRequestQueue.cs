using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayGen.SUGAR.Client.RequestQueue
{
	public class ManualRequestQueue : IRequestQueue
	{
		private readonly Queue<Action> _responses = new Queue<Action>();
		private readonly Queue<QueueItem> _requests = new Queue<QueueItem>();

		public void EnqueueRequest(
			Action request, 
			Action onSuccess, 
			Action<Exception> onError)
		{
			var item = new QueueItem(request, onSuccess, onError);
			_requests.Enqueue(item);
		}

		public void EnqueueRequest<TResult>(
			Func<TResult> request, 
			Action<TResult> onSuccess,
			Action<Exception> onError)
		{
			var item = new QueueItem<TResult>(request, onSuccess, onError);
			_requests.Enqueue(item);
		}

		public void Clear()
		{
			_responses.Clear();
			_requests.Clear();
		}

		public bool TryExecuteResponse()
		{
			Action response = null;
			var didDequeue = false;

			if (_responses.Count > 0)
			{
				response = _responses.Dequeue();
				didDequeue = true;
			}

			response?.Invoke();
			return didDequeue;
		}

		public bool ProcessPending()
		{
			var didProcess = false;

			if (_requests.Any())
			{
				var item = _requests.Dequeue();
				try
				{
					item.Request();
					_responses.Enqueue(item.OnSuccess);
				}
				catch (Exception e)
				{
					_responses.Enqueue(() => item.OnError?.Invoke(e));
				}

				didProcess = true;
			}

			return didProcess;
		}
	}
}