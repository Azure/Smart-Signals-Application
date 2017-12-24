﻿//-----------------------------------------------------------------------
// <copyright file="AlertRuleStore.cs" company="Microsoft Corporation">
//        Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Azure.Monitoring.SmartSignals.Shared.AlertRules
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Monitoring.SmartSignals.Shared.AzureStorage;
    using Microsoft.WindowsAzure.Storage.Table;
    using NCrontab;

    /// <summary>
    /// Implementation of the <see cref="IAlertRuleStore"/> using Azure table.
    /// </summary>
    public class AlertRuleStore : IAlertRuleStore
    {
        private const string TableName = "alertrules";
        private const string PartitionKey = "rules";

        private readonly ICloudTableWrapper alertRulesTable;
        private readonly ITracer tracer;

        /// <summary>
        /// Initializes a new instance of the<see cref="AlertRuleStore"/> class.
        /// </summary>
        /// <param name="tableClient">The azure storage table client</param>
        /// <param name="tracer">Log wrapper</param>
        public AlertRuleStore(ICloudTableClientWrapper tableClient, ITracer tracer)
        {
            this.tracer = tracer;

            // create the cloud table instance
            this.alertRulesTable = tableClient.GetTableReference(TableName);
            this.alertRulesTable.CreateIfNotExists();
        }

        /// <summary>
        /// Gets all the alert rules from the store.
        /// </summary>
        /// <returns>A <see cref="IList{AlertRule}"/> containing all the alert rules in the store.</returns>
        public async Task<IList<AlertRule>> GetAllAlertRulesAsync()
        {
            this.tracer.TraceInformation("Getting all smart signal alert rules");
            var alertRulesEntities = await this.alertRulesTable.ReadPartitionAsync<AlertRuleEntity>(PartitionKey);
            this.tracer.TraceInformation($"Found {alertRulesEntities.Count} alert rules");

            this.tracer.TraceVerbose($"Found alert rules: {string.Join(", ", alertRulesEntities.Select(e => e.RowKey))}");

            return alertRulesEntities.Select(entity => new AlertRule
            {
                Id = entity.RowKey,
                SignalId = entity.SignalId,
                ResourceType = entity.ResourceType,
                Schedule = CrontabSchedule.Parse(entity.CrontabSchedule)
            }).ToList();
        }

        /// <summary>
        /// Adds or updates an alert rule in the store.
        /// </summary>
        /// <param name="alertRule">The alert rule to add to the store.</param>
        /// <returns>A <see cref="System.Threading.Tasks.Task"/> object that represents the asynchronous operation.</returns>
        public async Task AddOrReplaceAlertRuleAsync(AlertRule alertRule)
        {
            // Execute the update operation
            this.tracer.TraceInformation($"updating alert rule: {alertRule.Id} for signal: {alertRule.SignalId}");
            var operation = TableOperation.InsertOrReplace(new AlertRuleEntity
            {
                PartitionKey = PartitionKey,
                RowKey = alertRule.Id,
                SignalId = alertRule.SignalId,
                ResourceType = alertRule.ResourceType,
                CrontabSchedule = alertRule.Schedule.ToString()
            });

            await this.alertRulesTable.ExecuteAsync(operation);

            this.tracer.TraceInformation($"updated alert rule: {alertRule.Id}");
        }
    }
}
