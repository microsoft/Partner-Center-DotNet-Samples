// -----------------------------------------------------------------------
// <copyright file="CreateSelfServePolicies.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.SelfServePolicies
{
    using Microsoft.Store.PartnerCenter.Exceptions;
    using Microsoft.Store.PartnerCenter.Models.SelfServePolicies;
    using System;

    /// <summary>
    /// A scenario that creates a self serve policy for a customer.
    /// </summary>
    public class CreateSelfServePolicies : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSelfServePolicies"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CreateSelfServePolicies(IScenarioContext context) : base("Create self serve policies", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string customerIdAsEntity = this.ObtainCustomerId("Enter the ID of the customer to create a policy for");
            string partnerIdAsGrantor = this.ObtainCustomerId("Enter the ID of the partner to create a policy for (note, this is the partner logged in with)");

            var partnerOperations = this.Context.UserPartnerOperations;

            var selfServePolicy = new SelfServePolicy
            {
                SelfServeEntity = new SelfServeEntity
                {
                    SelfServeEntityType = "customer",
                    TenantID = customerIdAsEntity,
                },
                Grantor = new Grantor
                {
                    GrantorType = "billToPartner",
                    TenantID = partnerIdAsGrantor,
                },
                Permissions = new Permission[]
                {
                    new Permission
                    {
                    Action = "Purchase",
                    Resource = "AzureReservedInstances",
                    },
                },
            };


            try
            {
                this.Context.ConsoleHelper.StartProgress($"Creating self serve policy between partnerId { partnerIdAsGrantor } and customerId { customerIdAsEntity }");
                // creates the self serve policy
                SelfServePolicy createdSelfServePolicy = partnerOperations.SelfServePolicies.Create(selfServePolicy);
                this.Context.ConsoleHelper.StopProgress();
                this.Context.ConsoleHelper.WriteObject(createdSelfServePolicy, $"Created self serve policy between partnerId { partnerIdAsGrantor } and customerId { customerIdAsEntity }");
            }
            catch (PartnerException partnerException)
            {
                this.Context.ConsoleHelper.StopProgress();

                if (partnerException.ServiceErrorPayload.ErrorCode.Equals("600041", StringComparison.InvariantCultureIgnoreCase))
                {
                    this.Context.ConsoleHelper.WriteColored($"Self serve policy between partnerId { partnerIdAsGrantor } and customerId { customerIdAsEntity } already exists", ConsoleColor.Yellow);
                }
            }
        }
    }
}
