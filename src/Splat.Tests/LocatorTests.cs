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

            Assert.IsType<DebugLogger>(logger);
            Assert.IsType<DefaultLogManager>(logManager);
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

            Assert.IsType<DebugLogger>(logger);
            Assert.IsType<DefaultLogManager>(logManager);
        }

        /// <summary>
        /// Tests to make sure that the locator's fire the resolver changed notifications.
        /// </summary>
        [Fact]
        public void WithoutSuppressNotificationsHappen()
        {
            int numberNotifications = 0;
            Action notificationAction = () => numberNotifications++;

            Locator.RegisterResolverCallbackChanged(notificationAction);

            Locator.Current = new ModernDependencyResolver();
            Locator.Current = new ModernDependencyResolver();

            // 2 for the changes, 1 for the callback being immediately called.
            Assert.Equal(3, numberNotifications);
        }

        /// <summary>
        /// Tests to make sure that the locator's don't fire the resolver changed notifications if they are suppressed.
        /// </summary>
        [Fact]
        public void WithSuppressionNotificationsDontHappen()
        {
            using (Locator.SuppressResolverCallbackChangedNotifications())
            {
                int numberNotifications = 0;
                Action notificationAction = () => numberNotifications++;

                Locator.RegisterResolverCallbackChanged(notificationAction);

                Locator.Current = new ModernDependencyResolver();
                Locator.Current = new ModernDependencyResolver();

                Assert.Equal(0, numberNotifications);
            }
        }

        /// <summary>
        /// Tests to make sure that the locator's don't fire the resolver changed notifications if we use WithResolver().
        /// </summary>
        [Fact]
        public void WithResolverNotificationsDontHappen()
        {
            int numberNotifications = 0;
            Action notificationAction = () => numberNotifications++;

            Locator.RegisterResolverCallbackChanged(notificationAction);

            using (Locator.Current.WithResolver())
            {
                using (Locator.Current.WithResolver())
                {
                }
            }

            // 1 due to the fact the callback is called when we register.
            Assert.Equal(1, numberNotifications);
        }

        /// <summary>
        /// Tests to make sure that the locator's don't fire the resolver changed notifications if we use WithResolver().
        /// </summary>
        [Fact]
        public void WithResolverNotificationsNotSuppressedHappen()
        {
            int numberNotifications = 0;
            Action notificationAction = () => numberNotifications++;

            Locator.RegisterResolverCallbackChanged(notificationAction);

            using (Locator.Current.WithResolver(false))
            {
                using (Locator.Current.WithResolver(false))
                {
                }
            }

            // 1 due to the fact the callback is called when we register.
            // 2 for, 1 for change to resolver, 1 for change back
            // 2 for, 1 for change to resolver, 1 for change back
            Assert.Equal(5, numberNotifications);
        }
    }
}
