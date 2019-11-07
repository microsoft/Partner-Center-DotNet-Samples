// -----------------------------------------------------------------------
// <copyright file="GetCustomerProductsByTargetSegmentByReservationScope.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.CustomerProducts
{
    using System.Globalization;

    /// <summary>
    /// A scenario that retrieves all the products in a catalog view that apply to a customer and that target a specific segment and reservation scope.
    /// </summary>
    public class GetCustomerProductsByTargetSegmentByReservationScope : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomerProductsByTargetSegment"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetCustomerProductsByTargetSegmentByReservationScope(IScenarioContext context) : base("Get products for customer by segment by reservation scope", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var customerId = this.ObtainCustomerId("Enter the ID of the corresponding customer");
            string targetView = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the target view to get its supported products", "The target view can't be empty");
            string segment = this.Context.ConsoleHelper.ReadNonEmptyString("The segment to filter the products on", "The segment can't be empty");

            this.Context.ConsoleHelper.StartProgress(string.Format(CultureInfo.InvariantCulture, "Getting products in catalog view {0} by segment {1} for customer {2}", targetView, segment, customerId));
            var products = partnerOperations.Customers.ById(customerId).Products.ByTargetView(targetView).ByTargetSegment(segment).ByReservationScope("AzurePlan").Get();
            this.Context.ConsoleHelper.StopProgress();

            this.Context.ConsoleHelper.WriteObject(products, "Products for customer by segment by reservation scope");
        }
    }
}
