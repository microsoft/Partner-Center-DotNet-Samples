// -----------------------------------------------------------------------
// <copyright file="GetCustomerUserDetails.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    /// <summary>
    /// Gets a single customer user details.
    /// </summary>
    public class GetCustomerUserDetails : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerUserDetails"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerUserDetails(IScenarioContext context) : base("Get a customer user details", context)
        {
        }

        /// <summary>
        /// Executes the get customer user details scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer user Id.
            string selectedCustomerUserId = this.ObtainCustomerUserId("Enter the ID of the customer user to get details");

            // get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the corresponding customer to get customer user details");
            var partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Getting customer user detail");

            // Get customer user detail
            var selectedCustomerUser = partnerOperations.Customers.ById(selectedCustomerId).Users.ById(selectedCustomerUserId).Get();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(selectedCustomerUser, "Customer User detail");
        }
    }
}
