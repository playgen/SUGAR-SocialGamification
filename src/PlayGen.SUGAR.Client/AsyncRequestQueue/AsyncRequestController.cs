using System;
using System.Collections.Generic;
using System.Threading;

namespace PlayGen.SUGAR.Client.AsyncRequestQueue
{
    public class AsyncRequestController : IDisposable
    {
        public Exception Exception { get; private set; }

        private readonly Queue<QueueItem> _requests = new Queue<QueueItem>();
        private readonly Queue<Action> _responses = new Queue<Action>();
        private readonly AutoResetEvent _processRequestHandle = new AutoResetEvent(false);
        private readonly ManualResetEvent _abortHandle = new ManualResetEvent(false);
        private readonly Thread _worker;
        
        private bool _isDisposed;
        private object _requestsLock = new object();
        private object _responsesLock = new object();

        public int RequestCount => _requests.Count;
        public int ResponseCount => _responses.Count;

        public AsyncRequestController()
        {
            _worker = new Thread(RequestWorker);
            _worker.Start();
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

        private void EnqueueResponse(Action response)
        {
            lock (_responsesLock)
            {
                _responses.Enqueue(response);
            }
        }

        public bool TryExecuteResponse()
        {
            lock (_responsesLock)
            {
                if(_responses.Count > 0)
                { 
                    var response = _responses.Dequeue();
                    response();
                    return true;
                }

                return false;
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

        private void RequestWorker()
        {
            try
            {
                var handles = new WaitHandle[] {_processRequestHandle, _abortHandle};
                int signal;

                while (true)
                {
                    
                    signal = WaitHandle.WaitAny(handles);

                    if (signal == 1)
                    {
                        break;
                    }

                    QueueItem item = null;
                    lock (_requestsLock)
                    {
                        if (_requests.Count > 0)
                        {
                            item = _requests.Dequeue();
                        }
                    }

                    if(item != null)
                    { 
                        try
                        {
                            item.Request();
                            EnqueueResponse(item.OnSuccess);
                        }
                        catch (Exception e)
                        {
                            EnqueueResponse(() => item.OnError(e));
                        }
                    }

                    if (_requests.Count > 0)
                    {
                        _processRequestHandle.Set();
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
