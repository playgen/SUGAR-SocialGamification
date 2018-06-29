using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace PlayGen.SUGAR.Server.Core.Tests
{
    public static class AssertUtil
    {
	    public static void ExecutionTimeAssert(Action method, int itterationCount, int expectedAverageMilliseconds)
	    {
		    var stopwatch = new Stopwatch();
		    var individualTimings = new long[itterationCount];

		    for (var i = 0; i < itterationCount; i++)
		    {
			    stopwatch.Restart();

			    method();

			    stopwatch.Stop();
			    individualTimings[i] = stopwatch.ElapsedMilliseconds;
		    }

		    var average = individualTimings.Average();

		    Assert.True(average <= expectedAverageMilliseconds, $"Expected average duration: {expectedAverageMilliseconds} but took: {average} milliseconds for {itterationCount} itterations." +
		                                                        $" \nTimings: {string.Join(", ", Array.ConvertAll(individualTimings, t => t.ToString()))}");
	    }
    }
}
