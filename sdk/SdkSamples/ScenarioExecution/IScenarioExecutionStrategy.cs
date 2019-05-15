// -----------------------------------------------------------------------
// <copyright file="IScenarioExecutionStrategy.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ScenarioExecution
{
    /// <summary>
    /// Defines the behavior to apply when a scenario is complete.
    /// </summary>
    public interface IScenarioExecutionStrategy
    {
        /// <summary>
        /// Determines whether the scenario is complete or it should be repeated.
        /// </summary>
        /// <param name="scenario">The scenario under consideration.</param>
        /// <returns>True is the scenario is complete, False is it should be repeated.</returns>
        bool IsScenarioComplete(IPartnerScenario scenario);
    }
}
