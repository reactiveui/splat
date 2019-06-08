using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Splat.Tests.Colors
{
    /// <summary>
    /// Unit Tests for Known Color logic.
    /// </summary>
    public class KnownColorTests
    {
        /// <summary>
        /// Gets the test data for FromKnownColor.
        /// </summary>
        public static IEnumerable<object[]> KnownColorEnums { get; } = XUnitHelpers.GetEnumAsTestTheory<KnownColor>();

        /// <summary>
        /// Tests to ensure a name is returned from a number akin to a KnownColor.
        /// </summary>
        /// <param name="knownColor">Known Color Enum to check.</param>
        [Theory]
        [MemberData(nameof(KnownColorEnums))]
        public void GetNameReturnsName(KnownColor knownColor)
        {
#if !NET_2_0
            if ((short)knownColor > 167)
            {
                // can't assess these.
                return;
            }
#endif

            var name = KnownColors.GetName(knownColor);
            Assert.False(string.IsNullOrWhiteSpace(name));
        }
    }
}
