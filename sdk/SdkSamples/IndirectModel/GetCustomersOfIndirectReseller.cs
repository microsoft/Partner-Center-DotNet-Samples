// -----------------------------------------------------------------------
// <copyright file="GetCustomersOfIndirectReseller.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.IndirectModel
{
    using System;
    using System.Globalization;
    using Models.Customers;
    using Models.Query;

    /// <summary>
    /// A scenario that retrieves the list of customers associated to an indirect reseller.
    /// </summary>
    public class GetCustomersOfIndirectReseller : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomersOfIndirectReseller"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomersOfIndirectReseller(IScenarioContext context) : base("Get customers of indirect reseller", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            string indirectResellerId = this.ObtainIndirectResellerId("Enter the ID of the indirect reseller: ");

            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;
            this.Context.ConsoleHelper.StartProgress("Getting customers of the indirect reseller");

            // Create a simple field filter.
            SimpleFieldFilter filter = new SimpleFieldFilter(
                CustomerSearchField.IndirectReseller.ToString(),
                FieldFilterOperation.StartsWith,
                indirectResellerId);

            // Create an iQuery object to pass to the Query method.
            IQuery myQuery = QueryFactory.Instance.BuildSimpleQuery(filter);

            // Get the collection of matching customers.
            Models.SeekBasedResourceCollection<Customer> customersPage = partnerOperations.Customers.Query(myQuery);

            this.Context.ConsoleHelper.StopProgress();

            // Create a customer enumerator which will aid us in traversing the customer pages.
            Enumerators.IResourceCollectionEnumerator<Models.SeekBasedResourceCollection<Customer>> customersEnumerator = partnerOperations.Enumerators.Customers.Create(customersPage);
            int pageNumber = 1;

            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Customers of indirect reseller: {0}", indirectResellerId));

            while (customersEnumerator.HasValue)
            {
                // Print the current customer results page.
                this.Context.ConsoleHelper.WriteObject(customersEnumerator.Current, string.Format(CultureInfo.InvariantCulture, "Customer Page: {0}", pageNumber++));

                Console.WriteLine();
                Console.Write("Press any key to retrieve the next customers page");
                Console.ReadKey();

                this.Context.ConsoleHelper.StartProgress("Getting next customers page");

                // Get the next page of customers.
                customersEnumerator.Next();

                this.Context.ConsoleHelper.StopProgress();
                Console.Clear();
            }
        }
    }
}
