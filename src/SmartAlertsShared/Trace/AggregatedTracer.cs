﻿namespace Microsoft.Azure.Monitoring.SmartAlerts.Shared.Trace
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implementation of the <see cref="ITracer"/> interface that traces to other <see cref="ITracer"/> objects.
    /// </summary>
    public partial class AggregatedTracer : ITracer
    {
        private readonly ConcurrentDictionary<ITracer, Type> _tracers;

        /// <summary>
        /// Initialized a new instance of the <see cref="AggregatedTracer"/> class.
        /// </summary>
        /// <param name="tracers">List of tracers to trace to</param>
        public AggregatedTracer(List<ITracer> tracers)
        {
            Diagnostics.EnsureArgumentNotNull(() => tracers);
            _tracers = new ConcurrentDictionary<ITracer, Type>(tracers.Where(t => t != null).ToDictionary(t => t, t => t.GetType()));

            Diagnostics.EnsureArgument(_tracers.Count > 0, () => tracers, "Must get at least one non-null tracer");
            Diagnostics.EnsureArgument(_tracers.Keys.Select(t => t.SessionId).Distinct().Count() == 1, () => tracers, "All tracers must have the same session ID");
            this.SessionId = _tracers.First().Key.SessionId;
            this.OperationHandler = new AggregateOperationHandler(_tracers.Keys.Select(t => t.OperationHandler));
        }

        #region Implementation of ITracer

        /// <summary>
        /// Gets the tracer's operation handler
        /// </summary>
        public ITelemetryOperationHandler OperationHandler { get; }

        /// <summary>
        /// Gets the tracer's session ID
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        /// Trace <paramref name="message"/> as Information message.
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="properties">Named string values you can use to classify and filter traces</param>
        public void TraceInformation(string message, IDictionary<string, string> properties = null)
        {
            this.SafeCallTracers(t => t.TraceInformation(message, properties));
        }

        /// <summary>
        /// Trace <paramref name="message"/> as Error message.
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="properties">Named string values you can use to classify and filter traces</param>
        public void TraceError(string message, IDictionary<string, string> properties = null)
        {
            this.SafeCallTracers(t => t.TraceError(message, properties));
        }

        /// <summary>
        /// Trace <paramref name="message"/> as Verbose message.
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="properties">Named string values you can use to classify and filter traces</param>
        public void TraceVerbose(string message, IDictionary<string, string> properties = null)
        {
            this.SafeCallTracers(t => t.TraceVerbose(message, properties));
        }

        /// <summary>
        /// Trace <paramref name="message"/> as Warning message.
        /// </summary>
        /// <param name="message">The message to trace</param>
        /// <param name="properties">Named string values you can use to classify and filter traces</param>
        public void TraceWarning(string message, IDictionary<string, string> properties = null)
        {
            this.SafeCallTracers(t => t.TraceWarning(message, properties));
        }

        /// <summary>
        /// Reports a metric.
        /// </summary>
        /// <param name="name">The metric name</param>
        /// <param name="value">The metric value</param>
        /// <param name="properties">Named string values you can use to classify and filter metrics</param>
        /// <param name="count">The aggregated metric count</param>
        /// <param name="max">The aggregated metric max value</param>
        /// <param name="min">The aggregated metric min name</param>
        /// <param name="timestamp">The aggregated metric timestamp</param>
        public void ReportMetric(string name, double value, IDictionary<string, string> properties = null, int? count = null, double? max = null, double? min = null, DateTime? timestamp = null)
        {
            this.SafeCallTracers(t => t.ReportMetric(name, value, properties, count, max, min, timestamp));
        }

        /// <summary>
        /// Reports a runtime exception.
        /// It uses exception and trace entities with same operation id.
        /// </summary>
        /// <param name="exception">The exception to report</param>
        public void ReportException(Exception exception)
        {
            this.SafeCallTracers(t => t.ReportException(exception));
        }

        /// <summary>
        /// Send information about a dependency handled by the application.
        /// </summary>
        /// <param name="dependencyName">The dependency name.</param>
        /// <param name="commandName">The command name</param>
        /// <param name="startTime">The dependency call start time</param>
        /// <param name="duration">The time taken by the application to handle the dependency.</param>
        /// <param name="success">Was the dependency call successful</param>
        /// <param name="metrics">Named double values that add additional metric info to the dependency</param>
        /// <param name="properties">Named string values you can use to classify and filter dependencies</param>
        public void TrackDependency(
            string dependencyName,
            string commandName,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success,
            IDictionary<string, double> metrics = null,
            IDictionary<string, string> properties = null)
        {
            this.SafeCallTracers(t => t.TrackDependency(dependencyName, commandName, startTime, duration, success, metrics, properties));
        }

        /// <summary>
        /// Send information about an event.
        /// </summary>
        /// <param name="eventName">The event name.</param>
        /// <param name="properties">Named string values you can use to classify and filter dependencies</param>
        /// <param name="metrics">Dictionary of application-defined metrics</param>
        public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            this.SafeCallTracers(t => t.TrackEvent(eventName, properties, metrics));
        }

        /// <summary>
        /// Creates a new activity scope, and returns an <see cref="IDisposable"/> activity.
        /// </summary>
        /// <returns>A disposable activity object to control the end of the activity scope</returns>
        public ITraceActivity CreateNewActivityScope()
        {
            return new AggregatedTraceActivity(_tracers.Keys.Select(t => t.CreateNewActivityScope()));
        }

        /// <summary>
        /// Flushes the telemetry channel
        /// </summary>
        public void Flush()
        {
            this.SafeCallTracers(t => t.Flush());
        }

        #endregion

        /// <summary>
        /// Runs <paramref name="action"/> on all aggregated tracers in a safe way
        /// </summary>
        /// <param name="action">The action to run</param>
        private void SafeCallTracers(Action<ITracer> action)
        {
            foreach (ITracer tracer in _tracers.Keys)
            {
                try
                {
                    action(tracer);
                }
                catch (Exception e)
                {
                    Type tracerType;
                    _tracers.TryRemove(tracer, out tracerType);
                    if (_tracers.Count == 0)
                    {
                        throw;
                    }

                    this.SafeCallTracers(t => t.TraceWarning($"Got exception while calling tracer {tracerType.Name}: {e}"));
                }
            }
        }
    }
}