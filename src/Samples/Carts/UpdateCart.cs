// -----------------------------------------------------------------------
// <copyright file="UpdateCart.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Carts
{
    using System.Linq;
    using Store.PartnerCenter.Models.Carts;
    
    /// <summary>
    /// A scenario that updates a new cart for a customer.
    /// </summary>
    public class UpdateCart : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCart"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateCart(IScenarioContext context) : base("Update a Cart", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            var partnerOperations = this.Context.UserPartnerOperations;

            string customerId = this.ObtainCustomerId("Enter the ID of the customer making the purchase");
            string cartId = this.ObtainCartID("Enter the ID of cart for which changes are to be made");
            int quantityChange = int.Parse(this.ObtainQuantity("Enter the amount the quantity has to be changed"));

            Cart existingCart = partnerOperations.Customers.ById(customerId).Carts.ById(cartId).Get();

            this.Context.ConsoleHelper.WriteObject(existingCart, "Cart to be updated");
            this.Context.ConsoleHelper.StartProgress("Updating cart");

            existingCart.LineItems.ToArray()[0].Quantity += quantityChange;
            
            var updatedCart = partnerOperations.Customers.ById(customerId).Carts.ById(cartId).Put(existingCart);
            this.Context.ConsoleHelper.StopProgress();
            this.Context.ConsoleHelper.WriteObject(updatedCart, "Updated cart");
        }
    }
}