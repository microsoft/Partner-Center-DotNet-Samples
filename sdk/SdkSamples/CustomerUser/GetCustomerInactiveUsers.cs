// -----------------------------------------------------------------------
// <copyright file="GetCustomerInactiveUsers.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    using System;
    using Models.Query;

    /// <summary>
    /// Gets inactive customer users in pages.
    /// </summary>
    public class GetCustomerInactiveUsers : BasePartnerScenario
    {
        /// <summary>
        /// The customer user page size.
        /// </summary>
        private int customerUserPageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerInactiveUsers"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        /// <param name="customeruserPageSize">The number of inactive customer users to return per page.</param>
        public GetCustomerInactiveUsers(IScenarioContext context, int customeruserPageSize = 0) : base("Get Paged inactive customer users", context)
        {
            this.customerUserPageSize = customeruserPageSize;
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get all inactive customer users in pages");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            // get customer user page size
            string customerUserPageSize = this.ObtainCustomerUserPageSize();
            this.customerUserPageSize = int.Parse(customerUserPageSize);

            SimpleFieldFilter filter = new SimpleFieldFilter("UserStatus", FieldFilterOperation.Equals, "Inactive");

            // Read inactive customer users in a batch
            this.Context.ConsoleHelper.StartProgress("Querying first page of inactive customer users");
            IQuery simpleQueryWithFilter = QueryFactory.Instance.BuildIndexedQuery(this.customerUserPageSize, 0, filter);
            Models.SeekBasedResourceCollection<Models.Users.CustomerUser> customerUsers = partnerOperations.Customers.ById(selectedCustomerId).Users.Query(simpleQueryWithFilter);
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.StartProgress("Creating customer user Enumerator");
            Enumerators.IResourceCollectionEnumerator<Models.SeekBasedResourceCollection<Models.Users.CustomerUser>> customerUsersEnumerator = partnerOperations.Enumerators.CustomerUsers.Create(customerUsers);
            this.Context.ConsoleHelper.StopProgress();

            while (customerUsersEnumerator.HasValue)
            {
                // print the current customer user results
                this.Context.ConsoleHelper.WriteObject(customerUsersEnumerator.Current);

                Console.WriteLine();
                Console.Write("Press any key to retrieve the next customer users page");
                Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine("Getting Next Page");

                // get the next page of customer users
                customerUsersEnumerator.Next();
                Console.Clear();
            }
        }
    }
}
