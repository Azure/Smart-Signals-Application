﻿//-----------------------------------------------------------------------
// <copyright file="SmartSignalRunnerMain.cs" company="Microsoft Corporation">
//        Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SmartSignalRunnerChildProcess
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Monitoring.SmartSignals.Analysis;
    using Microsoft.Azure.Monitoring.SmartSignals.Analysis.DetectionPresentation;
    using Microsoft.Azure.Monitoring.SmartSignals.Shared;
    using Microsoft.Azure.Monitoring.SmartSignals.Shared.ChildProcess;
    using Unity;

    /// <summary>
    /// The main class of the process that runs the smart signal
    /// </summary>
    public static class SmartSignalRunnerMain
    {
        private static IUnityContainer container;
        
        /// <summary>
        /// The main method
        /// </summary>
        /// <param name="args">Command line arguments. These arguments are expected to be created by <see cref="IChildProcessManager.RunChildProcessAsync{TOutput}"/>.</param>
        public static void Main(string[] args)
        {
            // Inject dependencies
            container = AnalysisDependenciesInjector.GetContainer()
                .WithSmartSignalRunner<SmartSignalRunner>()
                .WithTracer(null, true);

            // Run the analysis
            IChildProcessManager childProcessManager = container.Resolve<IChildProcessManager>();
            childProcessManager.RunAndListenToParentAsync<SmartSignalRequest, List<SmartSignalDetectionPresentation>>(args, RunSignalAsync).Wait();
        }

        /// <summary>
        /// Run the signal, by delegating the call to the registered <see cref="ISmartSignalRunner"/>
        /// </summary>
        /// <param name="request">The signal request</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>A <see cref="Task{TResult}"/>, returning the detections generated by the signal</returns>
        private static async Task<List<SmartSignalDetectionPresentation>> RunSignalAsync(SmartSignalRequest request, CancellationToken cancellationToken)
        {
            ISmartSignalRunner smartSignalRunner = container.Resolve<ISmartSignalRunner>();
            return await smartSignalRunner.RunAsync(request, cancellationToken);
        }
    }
}
