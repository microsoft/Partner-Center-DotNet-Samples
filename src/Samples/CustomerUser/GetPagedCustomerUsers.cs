// -----------------------------------------------------------------------
// <copyright file="GetPagedCustomerUsers.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerUser
{
    using System;
    using System.Globalization;
    using Models.Query;

    /// <summary>
    /// Gets customer users in pages.
    /// </summary>
    public class GetPagedCustomerUsers : BasePartnerScenario
    {
        /// <summary>
        /// The customer user page size.
        /// </summary>
        private int customerUserPageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagedCustomerUsers"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        /// <param name="customeruserPageSize">The number of customer users to return per page.</param>
        public GetPagedCustomerUsers(IScenarioContext context, int customeruserPageSize = 0) : base("Get Paged customer users", context)
        {
            this.customerUserPageSize = customeruserPageSize;
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get customer Id of the entered customer user.
            string selectedCustomerId = this.ObtainCustomerId("Enter the ID of the customer to get all customer users in pages");

            var partnerOperations = this.Context.UserPartnerOperations;

            // get customer user page size
            string customerUserPageSize = this.ObtainCustomerUserPageSize();
            this.customerUserPageSize = int.Parse(customerUserPageSize);

            this.Context.ConsoleHelper.StartProgress("Querying first page of customer users");

            // query the customers, get the first page if a page size was set, otherwise get all customers
            var customerUsersPage = (this.customerUserPageSize <= 0) ? partnerOperations.Customers.ById(selectedCustomerId).Users.Get() : partnerOperations.Customers.ById(selectedCustomerId).Users.Query(QueryFactory.Instance.BuildIndexedQuery(this.customerUserPageSize));
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.StartProgress("Creating customer user Enumerator");

            // create a customer user enumerator which will aid us in traversing the customer user pages
            var customerUsersEnumerator = partnerOperations.Enumerators.CustomerUsers.Create(customerUsersPage);
            this.Context.ConsoleHelper.StopProgress();
            int pageNumber = 1;
            while (customerUsersEnumerator.HasValue)
            {
                // print the current customer user result page
                this.Context.ConsoleHelper.WriteObject(customerUsersEnumerator.Current, string.Format(CultureInfo.InvariantCulture, "Customer User Page: {0}", pageNumber++));
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
