using System;
using System.Collections.Generic;

using lox.loxFunctions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.loxFunctions
{
    [TestClass]
    public class ClockTest
    {
        #region Fields

        private readonly Clock clock;

        private readonly MockDateTimeProvider dateTimeProvider;

        #endregion

        #region Constructors

        public ClockTest()
        {
            this.dateTimeProvider = new MockDateTimeProvider();
            this.clock = new Clock(this.dateTimeProvider);
        }

        #endregion

        #region Instance Methods

        [TestMethod]
        public void ClockHasArity0()
        {
            Assert.AreEqual(0,
                            this.clock.Arity());
        }

        [TestMethod]
        public void ClockReturnsSecondsSinceUnixEpoch()
        {
            var dayAfterUnixEpoch = new DateTimeOffset(1970,
                                                       1,
                                                       2,
                                                       0,
                                                       0,
                                                       0,
                                                       default);
            this.dateTimeProvider.MockUtcNow = dayAfterUnixEpoch;

            const double secondsPerDay = 86400.0d;

            var currentTime = this.clock.Call(new Interpreter(),
                                              new List<object?>(),
                                              new ReadOnlySpan<char>());

            Assert.AreEqual(secondsPerDay,
                            currentTime);
        }

        #endregion
    }

    public class MockDateTimeProvider : IDateTimeOffsetProvider
    {
        #region Fields

        public DateTimeOffset MockUtcNow;

        #endregion

        #region Instance Methods

        public DateTimeOffset UtcNow()
        {
            return this.MockUtcNow;
        }

        #endregion
    }
}