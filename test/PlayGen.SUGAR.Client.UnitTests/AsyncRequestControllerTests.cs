using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PlayGen.SUGAR.Client.AsyncRequestQueue;

namespace PlayGen.SUGAR.Client.UnitTests
{
    public class AsyncRequestControllerTests
    {
        [Test]
        public void ValueRequestsTriggerOnSuccessCallbacks()
        {
            // Arrange
            var onSuccessValues = new List<int>();
            var onErrorValues = new List<int>();

            var asyncRequestController = new AsyncRequestController();
            var values = Enumerable.Range(0, 1000).ToList();

            // Act
            values.ForEach(i =>
            {
                asyncRequestController.EnqueueRequest(() => i,
                    r => onSuccessValues.Add(r),
                    e => onErrorValues.Add(i));
            });

            var responseCount = 0;
            while (responseCount < values.Count)
            {
                if (asyncRequestController.TryExecuteResponse())
                {
                    responseCount++;
                }
            }

            // Assert
            CollectionAssert.IsEmpty(onErrorValues);
            CollectionAssert.AreEqual(values, onSuccessValues);
        }

        [Test]
        public void VoidRequestsTriggerOnSuccessCallbacks()
        {
            // Arrange
            var onSuccessVoids = new List<int>();
            var onErrorValues = new List<int>();

            var asyncRequestController = new AsyncRequestController();
            var values = Enumerable.Range(0, 1000).ToList();

            // Act
            values.ForEach(i =>
            {
                asyncRequestController.EnqueueRequest(() => { },
                    () => onSuccessVoids.Add(i),
                    e => onErrorValues.Add(i));
            });

            var responseCount = 0;
            while (responseCount < values.Count)
            {
                if (asyncRequestController.TryExecuteResponse())
                {
                    responseCount++;
                }
            }

            // Assert
            CollectionAssert.IsEmpty(onErrorValues);
            CollectionAssert.AreEqual(values, onSuccessVoids);
        }

        [Test]
        public void ErrorsTriggerOnErrorCallbacks()
        {
            // Arrange
            var onErrorValues = new List<int>();
            var onSuccessValues = new List<int>();

            var asyncRequestController = new AsyncRequestController();
            var values = Enumerable.Range(0, 1000).ToList();

            // Act
            values.ForEach(i =>
            {
                asyncRequestController.EnqueueRequest(() =>
                    {
                        throw new Exception($"{i}");
                        return i;
                    },
                    r => onSuccessValues.Add(r),
                    e => onErrorValues.Add(int.Parse(e.Message)));
            });

            var responseCount = 0;
            while (responseCount < values.Count)
            {
                if (asyncRequestController.TryExecuteResponse())
                {
                    responseCount++;
                }
            }

            // Assert
            CollectionAssert.IsEmpty(onSuccessValues);
            CollectionAssert.AreEqual(values, onErrorValues);
        }
    }
}
