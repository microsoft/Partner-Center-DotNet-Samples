// -----------------------------------------------------------------------
// <copyright file="IPartnerScenario.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a partner scenario that demos one or more related partner center APIs.
    /// </summary>
    public interface IPartnerScenario
    {
        /// <summary>
        /// Gets the scenario title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the children scenarios of the current scenario.
        /// </summary>
        IReadOnlyList<IPartnerScenario> Children { get; }

        /// <summary>
        /// Gets the scenario context.
        /// </summary>
        IScenarioContext Context { get; }

        /// <summary>
        /// Runs the scenario.
        /// </summary>
        void Run();
    }
}
