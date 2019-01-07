using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Splat.Tests
{
    /// <summary>
    /// Tests to confirm that the locator is working.
    /// </summary>
    public class LocatorTests
    {
        /// <summary>
        /// Tests if the registrations are not empty on no external registrations.
        /// </summary>
        [Fact]
        public void InitializeSplat_RegistrationsNotEmptyNoRegistrations()
        {
            var logManager = Locator.Current.GetService(typeof(ILogManager));
            var logger = Locator.Current.GetService(typeof(ILogger));

            Assert.NotNull(logManager);
            Assert.NotNull(logger);

            Assert.IsType(typeof(DebugLogger), logger);
            Assert.IsType(typeof(DefaultLogManager), logManager);
        }

        /// <summary>
        /// Tests that if we use a contract it returns null entries for that type.
        /// </summary>
        [Fact]
        public void InitializeSplat_ContractRegistrationsNullNoRegistration()
        {
            var logManager = Locator.Current.GetService(typeof(ILogManager), "test");
            var logger = Locator.Current.GetService(typeof(ILogger), "test");

            Assert.Null(logManager);
            Assert.Null(logger);
        }

        /// <summary>
        /// Tests using the extension methods that the retrieving of the default InitializeSplat() still work.
        /// </summary>
        [Fact]
        public void InitializeSplat_ExtensionMethodsNotNull()
        {
            var logManager = Locator.Current.GetService<ILogManager>();
            var logger = Locator.Current.GetService<ILogger>();

            Assert.NotNull(logManager);
            Assert.NotNull(logger);

            Assert.IsType(typeof(DebugLogger), logger);
            Assert.IsType(typeof(DefaultLogManager), logManager);
        }
    }
}
