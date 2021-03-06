﻿//-----------------------------------------------------------------------
// <copyright file="TelemetryResourceNotFoundException.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Microsoft.Azure.Monitoring.SmartDetectors
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An exception indicating the telemetry resource was not found
    /// </summary>
    [Serializable]
    public class TelemetryResourceNotFoundException : TelemetryDataClientException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryResourceNotFoundException"/> class.
        /// </summary>
        public TelemetryResourceNotFoundException()
            : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryResourceNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public TelemetryResourceNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryResourceNotFoundException"/> class
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The actual exception that was thrown when saving state</param>
        public TelemetryResourceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryResourceNotFoundException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized
        /// object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual
        /// information about the source or destination.</param>
        protected TelemetryResourceNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
