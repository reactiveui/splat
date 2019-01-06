using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
#if PORTABLE || WINDOWS_PHONE || NETFX_CORE
    /// <summary>
    /// Specifies whether a property should be localized.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class LocalizableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableAttribute"/> class.
        /// </summary>
        /// <param name="isLocalizable">If the value is localizable or not.</param>
        public LocalizableAttribute(bool isLocalizable)
        {
            IsLocalizable = isLocalizable;
        }

        /// <summary>
        /// Gets a value indicating whether a property should be localized.
        /// </summary>
        public bool IsLocalizable { get; }
    }
#endif
}
