using System;
using System.Collections.Generic;
using System.Threading;

namespace PlayGen.SUGAR.Client.AsyncRequestQueue
{
    public class AsyncRequestWorker : IDisposable
    {
        private readonly AutoResetEvent _processRequestHandle = new AutoResetEvent(false);
        private readonly ManualResetEvent _abortHandle = new ManualResetEvent(false);
        private readonly Queue<QueueItem> _requests = new Queue<QueueItem>();
        private readonly object _requestsLock = new object();
        private readonly Thread _worker;

        private bool _isDisposed;

        public Exception Exception { get; private set; }

        public event Action<Action> ResponseEvent; 

        public AsyncRequestWorker()
        {
            _worker = new Thread(DoWork);
            _worker.Start();
        }

        ~AsyncRequestWorker()
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

        internal void Clear()
        {
            lock (_requests)
            {
                _requests.Clear();
            }
        }

        private void EnqueueRequest(QueueItem item)
        {
            lock (_requestsLock)
            {
                _requests.Enqueue(item);
            }
            _processRequestHandle.Set();
        }

        private void DoWork()
        {
            try
            {
                var handles = new WaitHandle[] { _processRequestHandle, _abortHandle };
                int signal;

                while (true)
                {
                    signal = WaitHandle.WaitAny(handles);

                    if (signal == 1)
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
                            ResponseEvent(item.OnSuccess);
                        }
                        catch (Exception e)
                        {
                            ResponseEvent(() => item.OnError(e));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Exception = e;
            }
        }
    }
}
