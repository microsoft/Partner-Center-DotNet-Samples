// -----------------------------------------------------------------------
// <copyright file="IScenarioContext.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples
{
    using Configuration;
    using Helpers;
    using Store.PartnerCenter;

    /// <summary>
    /// Holds context properties useful to the scenarios.
    /// </summary>
    public interface IScenarioContext
    {
        /// <summary>
        /// Gets a partner operations instance which is user based authenticated.
        /// </summary>
        IAggregatePartner UserPartnerOperations { get; }

        /// <summary>
        /// Gets a partner operations instance which is application based authenticated.
        /// </summary>
        IAggregatePartner AppPartnerOperations { get; }
        
        /// <summary>
        /// Gets a configuration instance.
        /// </summary>
        ConfigurationManager Configuration { get; }

        /// <summary>
        /// Gets a console helper instance.
        /// </summary>
        ConsoleHelper ConsoleHelper { get; }
    }
}
