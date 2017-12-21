namespace Microsoft.Azure.Monitoring.SmartSignals.ManagementApi.EndpointsLogic
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// This class is the logic for the /detections endpoint.
    /// </summary>
    public class DetectionsLogic : IDetectionsLogic
    {
        /// <summary>
        /// Gets all the detections.
        /// </summary>
        /// <returns>The smart detections.</returns>
        public Task<IEnumerable<SmartSignalDetection>> GetAllDetectionsAsync()
        {
            return null;
        }
    }
}
