﻿//-----------------------------------------------------------------------
// <copyright file="TestTableAlertPropertyValue.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MonitoringApplianceEmulatorTests
{
    using Microsoft.Azure.Monitoring.SmartDetectors.AlertPresentation;
    using Microsoft.Azure.Monitoring.SmartDetectors.RuntimeEnvironment.Contracts.AlertProperties;

    /// <summary>
    /// Illustrate a class that represents a row of a <see cref="TableAlertProperty{T}"/> for test purpose.
    /// </summary>
    public class TestTableAlertPropertyValue
    {
        [TableColumn("First Name")]
        public string FirstName { get; set; }

        [TableColumn("Last Name")]
        public string LastName { get; set; }

        [TableColumn("Goals avg")]
        public double Goals { get; set; }
    }
}