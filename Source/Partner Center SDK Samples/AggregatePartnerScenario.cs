// -----------------------------------------------------------------------
// <copyright file="AggregatePartnerScenario.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using ScenarioExecution;

    /// <summary>
    /// A scenarios that is composed of one or more sub-scenarios.
    /// </summary>
    public class AggregatePartnerScenario : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregatePartnerScenario"/> class.
        /// </summary>
        /// <param name="title">The scenario title.</param>
        /// <param name="childScenarios">A list of child scenarios.</param>
        /// <param name="context">The scenario context.</param>
        public AggregatePartnerScenario(
            string title,
            IEnumerable<IPartnerScenario> childScenarios,
            IScenarioContext context) : base(title, context, new AggregateScenarioExecutionStrategy(), new List<IPartnerScenario>(childScenarios))
        {
        }

        /// <summary>
        /// Runs the aggregate scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // display the child scenarios
            for (int i = 0; i < this.Children.Count; ++i)
            {
                Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", i + 1, this.Children[i].Title));
            }
        }
    }
}