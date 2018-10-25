// -----------------------------------------------------------------------
// <copyright file="FilterCustomers.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Customers
{
    using Store.PartnerCenter.Models.Customers;
    using Store.PartnerCenter.Models.Query;

    /// <summary>
    /// Returns customers according to a provided filter.
    /// </summary>
    public class FilterCustomers : BasePartnerScenario
    {
        /// <summary>
        /// The search field.
        /// </summary>
        private readonly CustomerSearchField customerSearchField;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterCustomers"/> class.
        /// </summary>
        /// <param name="title">The scenario title.</param>
        /// <param name="customerSearchField">The search field.</param>
        /// <param name="context">The scenario context.</param>
        public FilterCustomers(string title, CustomerSearchField customerSearchField, IScenarioContext context) : base(title, context)
        {
            this.customerSearchField = customerSearchField;
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string searchPrefix = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the prefix to search for", "The entered prefix is empty");

            this.Context.ConsoleHelper.StartProgress("Filtering");

            var fieldFilter = new SimpleFieldFilter(
                this.customerSearchField.ToString(),
                FieldFilterOperation.StartsWith,
                searchPrefix);

            var myQuery = QueryFactory.Instance.BuildSimpleQuery(fieldFilter);
            
            var customers = partnerOperations.Customers.Query(myQuery);

            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(customers, "Customer matches");
        }
    }
}
