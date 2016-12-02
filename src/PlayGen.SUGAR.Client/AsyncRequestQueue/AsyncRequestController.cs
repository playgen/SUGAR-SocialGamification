using System;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Client.AsyncRequestQueue
{
    public class AsyncRequestController : IDisposable
    {
        private readonly AsyncRequestWorker _asyncRequestWorker = new AsyncRequestWorker();
        private readonly Queue<Action> _responses = new Queue<Action>();
        private readonly object _responsesLock = new object();

        private bool _isDisposed;

        public AsyncRequestController()
        {
            _asyncRequestWorker.ResponseEvent += EnqueueResponse;
        }

        ~AsyncRequestController()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _asyncRequestWorker.ResponseEvent -= EnqueueResponse;
            _asyncRequestWorker.Dispose();
        }

        public void EnqueueRequest(Action request, Action onSuccess, Action<Exception> onError)
        {
            _asyncRequestWorker.EnqueueRequest(request, onSuccess, onError);
        }

        public void EnqueueRequest<TResult>(Func<TResult> request, Action<TResult> onSuccess, Action<Exception> onError)
        {
            _asyncRequestWorker.EnqueueRequest(request, onSuccess, onError);
        }

        public bool TryExecuteResponse()
        {
            if (_asyncRequestWorker.Exception != null)
            {
                throw _asyncRequestWorker.Exception;
            }

            Action response = null;

            lock (_responsesLock)
            {
                if(_responses.Count > 0)
                { 
                    response = _responses.Dequeue();
                }
            }

            if (response != null)
            {
                response();
                return true;
            }

            return false;
        }

        private void EnqueueResponse(Action response)
        {
            lock (_responsesLock)
            {
                _responses.Enqueue(response);
            }
        }

        internal void Clear()
        {
            _asyncRequestWorker.Clear();

            lock (_responsesLock)
            {
                _responses.Clear();
            }
        }
    }
}
