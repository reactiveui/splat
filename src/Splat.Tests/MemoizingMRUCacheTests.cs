using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests
{
    /// <summary>
    /// Unit Tests for the Memoizing MRU Cache.
    /// </summary>
    public class MemoizingMRUCacheTests
    {
        /// <summary>
        /// Checks to ensure an Argument Null Exception is thrown.
        /// </summary>
        [Fact]
        public void ThrowsArgumentNullException()
        {
            var instance = GetTestInstance();
            Assert.Throws<ArgumentNullException>(() => instance.Get(null));
        }

        /// <summary>
        /// Checks to ensure a value is returned.
        /// </summary>
        [Fact]
        public void ReturnsValue()
        {
            var instance = GetTestInstance();
            var result = instance.Get("Test1");
            Assert.NotNull(result);
        }

        /// <summary>
        /// Checks to ensure a value is returned for 2 duplicate calls.
        /// </summary>
        [Fact]
        public void ReturnsSameValue()
        {
            var instance = GetTestInstance();
            var result1 = instance.Get("Test1");
            Assert.NotNull(result1);
            var result2 = instance.Get("Test1");
            Assert.NotNull(result2);
            Assert.Same(result1, result2);
        }

        /// <summary>
        /// Checks to ensure 2 different values are returned for 2 different calls.
        /// </summary>
        [Fact]
        public void ReturnsDifferentValues()
        {
            var instance = GetTestInstance();
            var result1 = instance.Get("Test1");
            Assert.NotNull(result1);
            var result2 = instance.Get("Test2");
            Assert.NotNull(result2);
            Assert.NotSame(result1, result2);
        }

        /// <summary>
        /// Crude test for checking thread safety when using Get.
        /// </summary>
        [Fact]
        public void ThreadSafeRetrievalTest()
        {
            var instance = GetTestInstance();

            var tests = Enumerable.Range(0, 100);

            var results = tests.AsParallel().Select(_ => instance.Get("Test1")).ToList();

            var first = results.First();

            foreach (var dummyObjectClass1 in results)
            {
                Assert.Same(first, dummyObjectClass1);
            }
        }

        private MemoizingMRUCache<string, DummyObjectClass1> GetTestInstance()
        {
            return new MemoizingMRUCache<string, DummyObjectClass1>(
                (param, o) => new DummyObjectClass1(),
                256);
        }
    }
}
