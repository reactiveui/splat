using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splat
{
    /// <summary>
    /// A disposable which will call the specified action.
    /// </summary>
    internal sealed class ActionDisposable : IDisposable
    {
        private Action _block;

        public ActionDisposable(Action block)
        {
            _block = block;
        }

        /// <summary>
        /// Gets a action disposable which does nothing.
        /// </summary>
        public static IDisposable Empty => new ActionDisposable(() => { });

        /// <inheritdoc />
        public void Dispose()
        {
            Interlocked.Exchange(ref _block, () => { })();
        }
    }
}
