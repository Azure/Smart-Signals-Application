﻿//-----------------------------------------------------------------------
// <copyright file="IDetectionPublisher.cs" company="Microsoft Corporation">
//        Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Azure.Monitoring.SmartSignals.Scheduler.Publisher
{
    using System.Collections.Generic;
    using Microsoft.Azure.Monitoring.SmartSignals;

    /// <summary>
    /// An interface for publishing detections
    /// </summary>
    public interface IDetectionPublisher
    {
        /// <summary>
        /// Publish detections as events to Application Insights
        /// </summary>
        /// <param name="signalId">The signal ID</param>
        /// <param name="detections">The detections to publish</param>
        void PublishDetections(string signalId, IList<SmartSignalDetection> detections);
    }
}
