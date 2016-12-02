using System;

namespace PlayGen.SUGAR.Client.AsyncRequestQueue
{
    public class QueueItem
    {
        public Action Request { get; protected set; }

        public Action OnSuccess { get; protected set; }

        public Action<Exception> OnError { get; protected set; }

        public QueueItem(Action request, Action onSuccess, Action<Exception> onError) : this(onError)
        {
            OnSuccess = onSuccess;
            Request = request;
        }

        protected QueueItem(Action<Exception> onError)
        {
            OnError = onError;
        }
    }

    public sealed class QueueItem<TResult> : QueueItem
    {
        private TResult Result;

        public QueueItem(Func<TResult> request, Action<TResult> onSuccess, Action<Exception> onError)
            : base(onError)
        {
            Request = () => Result = request();
            OnSuccess = () => onSuccess(Result);
        }
    }
}
