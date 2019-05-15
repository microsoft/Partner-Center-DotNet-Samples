// -----------------------------------------------------------------------
// <copyright file="AggregateScenarioExecutionStrategy.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.ScenarioExecution
{
    using System;
    using System.Globalization;
    using Helpers;

    /// <summary>
    /// An execution strategy which prompts the user to select a child scenario or exit the current scenario.
    /// </summary>
    public class AggregateScenarioExecutionStrategy : IScenarioExecutionStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateScenarioExecutionStrategy"/> class.
        /// </summary>
        public AggregateScenarioExecutionStrategy()
        {
        }

        /// <summary>
        /// Determines whether the scenario is complete or it should be repeated.
        /// </summary>
        /// <param name="scenario">The scenario under consideration.</param>
        /// <returns>True is the scenario is complete, False is it should be repeated.</returns>
        public bool IsScenarioComplete(IPartnerScenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException("scenario");
            }

            if (scenario.Children == null || scenario.Children.Count <= 0)
            {
                throw new ArgumentException("childScenarios must not be empty.");
            }

            int scenarioNumber = AggregateScenarioExecutionStrategy.ReadScenarioNumberFromConsole(scenario.Children.Count);

            if (scenarioNumber > 0)
            {
                // run the selected child scenario
                scenario.Children[scenarioNumber - 1].Run();

                // the current scenario should be restarted
                return false;
            }
            else
            {
                // user pressed escape, exit scenario
                return true;
            }
        }

        /// <summary>
        /// Reads user input from the console and extracts a scenario number from it. Returns 0 if the user entered nothing.
        /// </summary>
        /// <param name="maxScenarioNumber">The maximum scenario number to allow.</param>
        /// <returns>The scenario number the user entered or 0 if the user cancelled.</returns>
        private static int ReadScenarioNumberFromConsole(int maxScenarioNumber)
        {
            if (maxScenarioNumber < 1)
            {
                throw new ArgumentException("maxScenarioNumber must be at least 1");
            }

            int scenarioNumber;

            while (true)
            {
                ConsoleHelper.Instance.Warning("Enter the scenario number to run (press Q to exit to previous screen): ");
                string input = Console.ReadLine();
                
                if (input.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    scenarioNumber = 0;
                    break;
                }
                else if (int.TryParse(input, out scenarioNumber))
                {
                    if (scenarioNumber >= 1 && scenarioNumber <= maxScenarioNumber)
                    {
                        break;
                    }
                    else
                    {
                        ConsoleHelper.Instance.Error(string.Format(CultureInfo.InvariantCulture, "Enter a scenario number between 1 and {0}", maxScenarioNumber));
                    }
                }
            }

            return scenarioNumber;
        }
    }
}
