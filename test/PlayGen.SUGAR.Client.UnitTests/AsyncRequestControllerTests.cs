using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PlayGen.SUGAR.Client.AsyncRequestQueue;

namespace PlayGen.SUGAR.Client.UnitTests
{
    public class AsyncRequestControllerTests
    {
        private List<int> _onSuccessValues = new List<int>();
        private List<int> _onSuccessVoids = new List<int>();
        private List<int> _onErrorValues = new List<int>();

        [Test]
        public void ValueRequestsTriggerOnSuccessCallbacks()
        {
            // Arrange
            var asyncRequestController = new AsyncRequestController();
            var values = Enumerable.Range(0, 100).ToList();
            values.ForEach((i) =>
            {
                asyncRequestController.EnqueueRequest(() => i, 
                    OnSuccess,
                    OnError);
            });

            // Act
            while(asyncRequestController.TryExecuteResponse()
                || asyncRequestController.RequestCount > 0 
                || asyncRequestController.ResponseCount > 0)
            { }

            // Assert
            CollectionAssert.AreEqual(values, _onSuccessValues);
        }
        
        [Test]
        public void VoidRequestsTriggerOnSuccessCallbacks()
        {
            // Arrange
            var asyncRequestController = new AsyncRequestController();
            var values = Enumerable.Range(0, 100).ToList();
            values.ForEach((i) =>
            {
                asyncRequestController.EnqueueRequest(() => { },
                    () => _onSuccessVoids.Add(i),
                    OnError);
            });

            // Act
            while (asyncRequestController.TryExecuteResponse()
                || asyncRequestController.RequestCount > 0
                || asyncRequestController.ResponseCount > 0)
            { }

            // Assert
            CollectionAssert.AreEqual(values, _onSuccessVoids);
        }

        [Test]
        public void ErrorsTriggerOnErrorCallbacks()
        {
            // Arrange
            var asyncRequestController = new AsyncRequestController();
            var values = Enumerable.Range(0, 100).ToList();
            values.ForEach((i) =>
            {
                asyncRequestController.EnqueueRequest(() => 
                    {
                        throw new Exception($"{i}");
                        return i;
                    },
                    OnSuccess,
                    OnError);
            });

            // Act
            while (asyncRequestController.TryExecuteResponse()
                || asyncRequestController.RequestCount > 0
                || asyncRequestController.ResponseCount > 0)
            { }

            // Assert
            CollectionAssert.AreEqual(values, _onErrorValues);
        }

        private void OnSuccess(int result)
        {
            _onSuccessValues.Add(result);
        }

        private void OnError(Exception e)
        {
            _onErrorValues.Add(int.Parse(e.Message));
        }
    }
}
