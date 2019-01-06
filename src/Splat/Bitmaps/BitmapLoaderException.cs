using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// A exception that occurs when there is a problem using or retrieving the <see cref="IBitmapLoader"/>.
    /// </summary>
    [Serializable]
    public class BitmapLoaderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapLoaderException"/> class.
        /// </summary>
        public BitmapLoaderException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapLoaderException"/> class.
        /// </summary>
        /// <param name="message">The message about the exception.</param>
        public BitmapLoaderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapLoaderException"/> class.
        /// </summary>
        /// <param name="message">The message about the exception.</param>
        /// <param name="innerException">Any other internal exceptions we are mapping.</param>
        public BitmapLoaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BitmapLoaderException" /> class.</summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination. </param>
        protected BitmapLoaderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
