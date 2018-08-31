// -----------------------------------------------------------------------
// <copyright file="PromptExecutionStrategy.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ScenarioExecution
{
    using System;
    using Helpers;

    /// <summary>
    /// An scenario execution strategy that prompts the user repeat or exit the current scenario.
    /// </summary>
    public class PromptExecutionStrategy : IScenarioExecutionStrategy
    {
        /// <summary>
        /// Determines whether the scenario is complete or it should be repeated.
        /// </summary>
        /// <param name="scenario">The scenario under consideration.</param>
        /// <returns>True is the scenario is complete, False is it should be repeated.</returns>
        public bool IsScenarioComplete(IPartnerScenario scenario)
        {
            ConsoleHelper.Instance.Warning("Press Q return to the previous screen or R to repeat the current scenario:", false);

            ConsoleKeyInfo keyRead = Console.ReadKey(true);

            while (keyRead.Key != ConsoleKey.R && keyRead.Key != ConsoleKey.Q)
            {
                keyRead = Console.ReadKey(true);
            }

            return keyRead.Key == ConsoleKey.Q;
        }
    }
}
