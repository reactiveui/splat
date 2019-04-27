using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat.ApplicationPerformanceMonitoring
{
    /// <summary>
    /// Func based Feature Usage Tracking Manager.
    /// </summary>
    public class FuncFeatureUsageTrackingManager : IFeatureUsageTrackingManager
    {
        private readonly Func<string, IFeatureUsageTrackingSession> _featureUsageTrackingSessionFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncFeatureUsageTrackingManager"/> class.
        /// </summary>
        /// <param name="featureUsageTrackingSessionFunc">
        /// Factory function for a Feature Usage Tracking Session.
        /// </param>
        public FuncFeatureUsageTrackingManager(Func<string, IFeatureUsageTrackingSession> featureUsageTrackingSessionFunc)
        {
            _featureUsageTrackingSessionFunc = featureUsageTrackingSessionFunc ??
                                               throw new ArgumentNullException(nameof(featureUsageTrackingSessionFunc));
        }

        /// <inheritdoc/>
        public IFeatureUsageTrackingSession GetFeatureUsageTrackingSession(string featureName)
        {
            return _featureUsageTrackingSessionFunc(featureName);
        }
    }
}
