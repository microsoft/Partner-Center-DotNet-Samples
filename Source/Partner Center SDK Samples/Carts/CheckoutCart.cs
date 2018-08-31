// -----------------------------------------------------------------------
// <copyright file="CheckoutCart.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Carts
{
    /// <summary>
    /// A scenario that checkout a cart for a customer.
    /// </summary>
    public class CheckoutCart : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutCart"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public CheckoutCart(IScenarioContext context) : base("Checkout a Cart", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer making the purchase");
            string cartId = this.ObtainCartID("Enter the ID of cart to checkout");

            var existingCart = partnerOperations.Customers.ById(customerId).Carts.ById(cartId).Get();

            this.Context.ConsoleHelper.WriteObject(existingCart, "Cart to be checked out");
            this.Context.ConsoleHelper.StartProgress("Checking out cart");
            var checkoutResult = partnerOperations.Customers.ById(customerId).Carts.ById(cartId).Checkout();
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(checkoutResult, "Final Cart: ");
        }
    }
}