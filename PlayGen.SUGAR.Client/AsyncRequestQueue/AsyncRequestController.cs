using System;
using System.Collections.Generic;
using System.Threading;

namespace PlayGen.SUGAR.Client.AsyncRequestQueue
{
	public class AsyncRequestController : IDisposable
	{
		private readonly AutoResetEvent _processRequestHandle = new AutoResetEvent(false);
		private readonly ManualResetEvent _abortHandle = new ManualResetEvent(false);

		private readonly Queue<Action> _responses = new Queue<Action>();
		private readonly Queue<QueueItem> _requests = new Queue<QueueItem>();

		private readonly object _requestsLock = new object();
		private readonly object _responsesLock = new object();

		private readonly int _timeoutMilliseconds;
		private readonly QueueItem _onTimeoutItem;

		private Exception _workerException;
		private bool _isDisposed;

		public AsyncRequestController(int timeoutMilliseconds, Action onTimeout)
		{
			_timeoutMilliseconds = timeoutMilliseconds;
			_onTimeoutItem = new QueueItem(onTimeout, null, e => throw e);

			var worker = new Thread(DoWork);
			worker.Start();
		}

		~AsyncRequestController()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (_isDisposed) return;

			_abortHandle.Set();
			_isDisposed = true;
		}

		public void EnqueueRequest(Action request, Action onSuccess, Action<Exception> onError)
		{
			var item = new QueueItem(request, onSuccess, onError);
			EnqueueRequest(item);
		}

		public void EnqueueRequest<TResult>(Func<TResult> request, Action<TResult> onSuccess, Action<Exception> onError)
		{
			var item = new QueueItem<TResult>(request, onSuccess, onError);
			EnqueueRequest(item);
		}

		public void Clear()
		{
			lock (_requests)
			{
				_responses.Clear();
				_requests.Clear();
			}
		}

		public bool TryExecuteResponse()
		{
			if (_workerException != null)
			{
				throw _workerException;
			}

			Action response = null;
			var didDequeue = false;

			lock (_responsesLock)
			{
				if(_responses.Count > 0)
				{ 
					response = _responses.Dequeue();
					didDequeue = true;
				}
			}

			response?.Invoke();
			return didDequeue;
		}

		private void EnqueueResponse(Action response)
		{
			lock (_responsesLock)
			{
				_responses.Enqueue(response);
			}
		}

		private void EnqueueRequest(QueueItem item)
		{
			lock (_requestsLock)
			{
				_requests.Enqueue(item);
				_processRequestHandle.Set();
			}
		}

		private void EnqueueOnTimeoutItem()
		{
			lock (_requestsLock)
			{
				_requests.Enqueue(_onTimeoutItem);
			}
		}

		private void DoWork()
		{
			try
			{
				var handles = new WaitHandle[] { _processRequestHandle, _abortHandle };
				var abortHandleIndex = 1;

				while (true)
				{
					var signal = WaitHandle.WaitAny(handles, _timeoutMilliseconds);

					if (signal == WaitHandle.WaitTimeout)
					{
						EnqueueOnTimeoutItem();
					}
					else if (signal == abortHandleIndex)
					{
						break;
					}

					QueueItem item;
					lock (_requestsLock)
					{
						item = _requests.Dequeue();

						if (_requests.Count > 0)
						{
							_processRequestHandle.Set();
						}
					}

					if (item != null)
					{
						try
						{
							item.Request();
							EnqueueResponse(item.OnSuccess);
						}
						catch (Exception e)
						{
							EnqueueResponse(() => item.OnError?.Invoke(e));
						}
					}
				}
			}
			catch (Exception e)
			{
				_workerException = e;
			}
		}
	}
}
