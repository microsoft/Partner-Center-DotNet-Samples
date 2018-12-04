// -----------------------------------------------------------------------
// <copyright file="GetPagedCustomers.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    using System;
    using System.Globalization;
    using Store.PartnerCenter.Models.Query;

    /// <summary>
    /// Gets a partner customers in pages.
    /// </summary>
    public class GetPagedCustomers : BasePartnerScenario
    {
        /// <summary>
        /// The customer page size.
        /// </summary>
        private readonly int customerPageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagedCustomers"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        /// <param name="customerPageSize">The number of customers to return per page.</param>
        public GetPagedCustomers(IScenarioContext context, int customerPageSize = 0) : base("Get Paged customers", context)
        {
            this.customerPageSize = customerPageSize;
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Querying customers");

            // query the customers, get the first page if a page size was set, otherwise get all customers
            var customersPage = (this.customerPageSize <= 0) ? partnerOperations.Customers.Get() : partnerOperations.Customers.Query(QueryFactory.Instance.BuildIndexedQuery(this.customerPageSize));
            this.Context.ConsoleHelper.StopProgress();

            // create a customer enumerator which will aid us in traversing the customer pages
            var customersEnumerator = partnerOperations.Enumerators.Customers.Create(customersPage);
            int pageNumber = 1;

            while (customersEnumerator.HasValue)
            {
                // print the current customer results page
                this.Context.ConsoleHelper.WriteObject(customersEnumerator.Current, string.Format(CultureInfo.InvariantCulture, "Customer Page: {0}", pageNumber++));

                Console.WriteLine();
                Console.Write("Press any key to retrieve the next customers page");
                Console.ReadKey();

                this.Context.ConsoleHelper.StartProgress("Getting next customers page");

                // get the next page of customers
                customersEnumerator.Next();

                this.Context.ConsoleHelper.StopProgress();
                Console.Clear();
            }
        }
    }
}
