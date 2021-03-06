﻿//-----------------------------------------------------------------------
// <copyright file="ChartPropertyAttribute.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Azure.Monitoring.SmartDetectors.AlertPresentation
{
    /// <summary>
    /// An attribute defining the presentation of a chart property in an <see cref="Microsoft.Azure.Monitoring.SmartDetectors.Alert"/>.
    /// </summary>
    public class ChartPropertyAttribute : AlertPresentationPropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartPropertyAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The display name to use when presenting the property's value.</param>
        /// <param name="chartType">The type of the chart</param>
        /// <param name="xAxisType">The type of the X axis</param>
        /// <param name="yAxisType">The type of the Y axis</param>
        public ChartPropertyAttribute(
            string displayName,
            ChartType chartType,
            ChartAxisType xAxisType,
            ChartAxisType yAxisType)
            : base(displayName)
        {
            this.ChartType = chartType;
            this.XAxisType = xAxisType;
            this.YAxisType = yAxisType;
        }

        /// <summary>
        /// Gets the chart type
        /// </summary>
        public ChartType ChartType { get; }

        /// <summary>
        /// Gets the X axis type
        /// </summary>
        public ChartAxisType XAxisType { get; }

        /// <summary>
        /// Gets the Y axis type
        /// </summary>
        public ChartAxisType YAxisType { get; }
    }
}
