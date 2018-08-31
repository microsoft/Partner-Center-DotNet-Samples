// -----------------------------------------------------------------------
// <copyright file="GetEntitlements.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Entitlements
{
    using System;
    using System.Linq;
    using Models.Entitlements;

    /// <summary>
    /// Get customer entitlements.
    /// </summary>
    public class GetEntitlements : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetEntitlements"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetEntitlements(IScenarioContext context) : base("Get entitlements", context)
        {
        }

        /// <summary>
        /// Executes the get customer entitlements scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdToRetrieve = this.ObtainCustomerId("Enter the ID of the customer to retrieve entitlements for");

            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Retrieving customer entitlements");

            var entitlements = partnerOperations.Customers.ById(customerIdToRetrieve).Entitlements.Get();
            this.Context.ConsoleHelper.StopProgress();

            foreach (var entitlement in entitlements.Items)
            {
                this.Context.ConsoleHelper.WriteObject(entitlement, "Entitlement details");

                try
                {
                    switch (entitlement.EntitlementType.ToLowerInvariant())
                    {
                        case "reservedinstance":
                            var reservedInstanceArtifactDetailsLink =
                                ((ReservedInstanceArtifact)entitlement.EntitledArtifacts.FirstOrDefault(x => string.Equals(x.ArtifactType, "ReservedInstance", StringComparison.OrdinalIgnoreCase)))?.Link;

                            if (reservedInstanceArtifactDetailsLink != null)
                            {
                                var reservedInstanceArtifactDetails =
                                    reservedInstanceArtifactDetailsLink
                                        .InvokeAsync<ReservedInstanceArtifactDetails>(partnerOperations)
                                        .Result;
                                this.Context.ConsoleHelper.WriteObject(reservedInstanceArtifactDetails);
                            }

                            break;
                    }
                }
                catch (Exception ex)
                {
                    this.Context.ConsoleHelper.WriteObject(ex.Message, "Artifact Details");
                }
            }
        }
    }
}
