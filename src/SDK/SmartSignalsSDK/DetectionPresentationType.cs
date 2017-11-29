﻿namespace Microsoft.Azure.Monitoring.SmartSignals
{
    /// <summary>
    /// An enumeration of possible section where Smart Signal detection properties can be presented.
    /// </summary>
    public enum DetectionPresentationSection
    {
        /// <summary>
        /// Indicates a property belonging to the summary section. 
        /// For any detection, there must be exactly one property for this section.
        /// </summary>
        Summary,

        /// <summary>
        /// Indicates a property belonging to the properties section.
        /// </summary>
        Property,

        /// <summary>
        /// Indicates a property belonging to the analysis section.
        /// </summary>
        Analysis,

        /// <summary>
        /// Indicates a property belonging to the charts section.
        /// </summary>
        Chart,

        /// <summary>
        /// Indicates a property belonging to the additional queries section.
        /// </summary>
        AdditionalQuery
    }
}