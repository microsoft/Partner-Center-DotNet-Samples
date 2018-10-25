// -----------------------------------------------------------------------
// <copyright file="SortCustomerUsers.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    using System.Globalization;
    using Models.Query;

    /// <summary>
    /// Sorts the customer users.
    /// </summary>
    public class SortCustomerUsers : BasePartnerScenario
    {
        /// <summary>
        /// The sort direction.
        /// </summary>
        private readonly SortDirection sortDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortCustomerUsers"/> class.
        /// </summary>
        /// <param name="title">The scenario title.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="context">The scenario context.</param>
        public SortCustomerUsers(string title, SortDirection sortDirection, IScenarioContext context) : base(title, context)
        {
            this.sortDirection = sortDirection;
        }

        /// <summary>
        /// Executes the sort customer users scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer Id.
            string selectedCustomerId = this.ObtainCustomerId("Enter the customer ID");
            var partnerOperations = this.Context.UserPartnerOperations;

            // get sort field.
            string sortField = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the sort field (DisplayName,UserPrincipalName)", "The entered sort field is empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting customer users sorted by {0} in {1} order", sortField, this.sortDirection.ToString()));

            // get sorted customer users.
            var customerUsers = partnerOperations.Customers
                                                 .ById(selectedCustomerId)
                                                 .Users
                                                 .Query(QueryFactory.Instance.BuildIndexedQuery(20, sortOption: new Sort(sortField, this.sortDirection)));

            this.Context.ConsoleHelper.StopProgress();
            
            this.Context.ConsoleHelper.WriteObject(customerUsers, string.Format(CultureInfo.InvariantCulture, "Sorted Customer Users by {0} in {1} order", sortField, this.sortDirection.ToString()));
        }
    }
}