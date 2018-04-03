using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class AsyncRequestControllerTests : ClientTestBase
	{
		private IAsyncRequestController DefaultAsyncRequestController => new AsyncRequestController(60 * 1000, null);

		[Fact]
		public void ValueRequestsTriggerOnSuccessCallbacks()
		{
			// Arrange
			var onSuccessValues = new List<int>();
			var onErrorValues = new List<int>();

			using (var asyncRequestController = DefaultAsyncRequestController)
			{
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
				Assert.Empty(onErrorValues);
				Assert.Equal(values, onSuccessValues);
			}
		}

		[Fact]
		public void VoidRequestsTriggerOnSuccessCallbacks()
		{
			// Arrange
			var onSuccessVoids = new List<int>();
			var onErrorValues = new List<int>();

			using (var asyncRequestController = DefaultAsyncRequestController)
			{
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
				Assert.Empty(onErrorValues);
				Assert.Equal(values, onSuccessVoids);
			}
		}

		[Fact]
		public void ErrorsTriggerOnErrorCallbacks()
		{
			// Arrange
			var onErrorValues = new List<int>();
			var onSuccessValues = new List<int>();

			using (var asyncRequestController = DefaultAsyncRequestController)
			{
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
				Assert.Empty(onSuccessValues);
				Assert.Equal(values, onErrorValues);
			}
		}

		[Fact]
		public async void DoesTimeoutAction()
		{
			// Arrange
			var didTimeout = false;
			using (var asyncRequestController = new AsyncRequestController(100, () => didTimeout = true))
			{
				// Act
				var stopWatch = Stopwatch.StartNew();
				var timeout = 1 * 1000;

				while (!didTimeout && stopWatch.ElapsedMilliseconds < timeout)
				{
					await Task.Delay(100);
				}

				// Assert
				Assert.True(didTimeout);
			}
		}

		public AsyncRequestControllerTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}
