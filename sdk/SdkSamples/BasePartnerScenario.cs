// -----------------------------------------------------------------------
// <copyright file="BasePartnerScenario.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples
{
    using System;
    using System.Collections.Generic;
    using ScenarioExecution;

    /// <summary>
    /// The base class for partner scenarios. Provides common behavior for all partner scenarios.
    /// </summary>
    public abstract class BasePartnerScenario : IPartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePartnerScenario"/> class.
        /// </summary>
        /// <param name="title">The scenario title.</param>
        /// <param name="context">The scenario context.</param>
        /// <param name="executionStrategy">The scenario execution strategy.</param>
        /// <param name="childScenarios">The child scenarios attached to the current scenario.</param>
        public BasePartnerScenario(string title, IScenarioContext context, IScenarioExecutionStrategy executionStrategy = null, IReadOnlyList<IPartnerScenario> childScenarios = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("title has to be set");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.Title = title;
            this.Context = context;

            this.ExecutionStrategy = executionStrategy ?? new PromptExecutionStrategy();
            this.Children = childScenarios;
        }

        /// <summary>
        /// Gets the scenario title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the children scenarios of the current scenario.
        /// </summary>
        public IReadOnlyList<IPartnerScenario> Children { get; private set; }

        /// <summary>
        /// Gets the scenario context.
        /// </summary>
        public IScenarioContext Context { get; private set; }

        /// <summary>
        /// Gets or sets the scenario execution behavior.
        /// </summary>
        private IScenarioExecutionStrategy ExecutionStrategy { get; set; }

        /// <summary>
        /// Runs the scenario.
        /// </summary>
        public void Run()
        {
            do
            {
                Console.Clear();
                this.Context.ConsoleHelper.WriteColored(this.Title, ConsoleColor.DarkCyan);
                this.Context.ConsoleHelper.WriteColored(new string('-', 80), ConsoleColor.DarkCyan);
                Console.WriteLine();

                try
                {
                    this.RunScenario();
                }
                catch (Exception exception)
                {
                    this.Context.ConsoleHelper.StopProgress();
                    this.Context.ConsoleHelper.Error(exception.ToString());
                }

                Console.WriteLine();
            }
            while (!this.ExecutionStrategy.IsScenarioComplete(this));
        }

        /// <summary>
        /// Obtains a customer ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>A customer ID.</returns>
        protected string ObtainCustomerId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultCustomerId,
                "Customer Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the customer ID" : promptMessage,
                "The customer ID can't be empty");
        }

        /// <summary>
        /// Obtains a configuration policy ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>A configuration policy ID.</returns>
        protected string ObtainConfigurationPolicyId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultConfigurationPolicyId,
                "Configuration Policy Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the configuration policy ID" : promptMessage,
                "The configuration policy ID can't be empty");
        }

        /// <summary>
        /// Obtains a device batch ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>A device batch ID.</returns>
        protected string ObtainDeviceBatchId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultDeviceBatchId,
                "Device Batch Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the device batch ID" : promptMessage,
                "The device batch ID can't be empty");
        }

        /// <summary>
        /// Obtains a device ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>A device ID.</returns>
        protected string ObtainDeviceId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                 this.Context.Configuration.Scenario.DefaultDeviceId,
                 "Device Id",
                 string.IsNullOrWhiteSpace(promptMessage) ? "Enter the device ID" : promptMessage,
                 "The device ID can't be empty");
        }

        /// <summary>
        /// Obtains a tracking ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>A batch upload status tracking ID.</returns>
        protected string ObtainBatchUploadStatusTrackingId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                 this.Context.Configuration.Scenario.DefaultBatchUploadStatusTrackingId,
                 "Batch Upload Status Tracking Id",
                 string.IsNullOrWhiteSpace(promptMessage) ? "Enter the Batch Upload Status Tracking ID" : promptMessage,
                 "The Batch Upload Status Tracking ID can't be empty");
        }

        /// <summary>
        /// Obtains an indirect reseller ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>An indirect reseller ID.</returns>
        protected string ObtainIndirectResellerId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultIndirectResellerId,
                "Indirect Reseller Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the indirect reseller ID" : promptMessage,
                "The indirect reseller ID can't be empty");
        }

        /// <summary>
        /// Obtains a directory role ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>A directory role ID.</returns>
        protected string ObtainDirectoryRoleId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultDirectoryRoleId,
                "Directory Role Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the Directory Role ID" : promptMessage,
                "The Directory Role ID can't be empty");
        }

        /// <summary>
        /// Obtains a customer user ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>A customer user ID.</returns>
        protected string ObtainCustomerUserId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultCustomerUserId,
                "Customer User Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the customer user ID" : promptMessage,
                "The customer user ID can't be empty");
        }

        /// <summary>
        /// Obtains a user member ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>A user member ID.</returns>
        protected string ObtainUserMemberId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultUserMemberId,
                "User Member Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the User Member ID" : promptMessage,
                "The User Member ID can't be empty");
        }

        /// <summary>
        /// Obtains a customer user ID to delete from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>A customer user ID.</returns>
        protected string ObtainCustomerUserIdDelete(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.CustomerUserIdToDelete,
                "Customer User Id To Delete",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the customer user ID to delete" : promptMessage,
                "The customer user ID can't be empty");
        }

        /// <summary>
        /// Obtains a customer user page size to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>A customer user page size.</returns>
        protected string ObtainCustomerUserPageSize(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.CustomerUserPageSize,
                "Customer user page size",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the customer user page size" : promptMessage,
                "The customer user page size can't be empty");
        }

        /// <summary>
        /// Obtains an MPN ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>The MPN ID.</returns>
        protected string ObtainMpnId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.PartnerMpnId,
                "MPN Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the MPN ID" : promptMessage,
                "The MPN ID can't be empty");
        }

        /// <summary>
        /// Obtains an offer ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>The offer ID.</returns>
        protected string ObtainOfferId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultOfferId,
                "Offer Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the offer ID" : promptMessage,
                "The Offer ID can't be empty");
        }

        /// <summary>
        /// Obtains a product ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>The product ID.</returns>
        protected string ObtainProductId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultProductId,
                "Product Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the product ID" : promptMessage,
                "The Product ID can't be empty");
        }

        /// <summary>
        /// Obtains a SKU ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>The SKU ID.</returns>
        protected string ObtainSkuId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultSkuId,
                "Sku Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the sku ID" : promptMessage,
                "The Sku ID can't be empty");
        }

        /// <summary>
        /// Obtains the availability ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>The availability ID.</returns>
        protected string ObtainAvailabilityId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultAvailabilityId,
                "Availability Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the availability ID" : promptMessage,
                "The Availability ID can't be empty");
        }

        /// <summary>
        /// Obtains a catalogItemId to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>The catalog Item ID.</returns>
        protected string ObtainCatalogItemId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultCatalogItemId,
                "Catalog Item Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the catalog item ID" : promptMessage,
                "The catalog item ID can't be empty");
        }

        /// <summary>
        /// Obtains a scope for provisioning status to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>The scope.</returns>
        protected string ObtainScope(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultScope,
                "Scope",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the Scope" : promptMessage,
                "The Scope can't be empty");
        }

        /// <summary>
        /// Obtain an order ID to work with the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message</param>
        /// <returns>The order ID</returns>
        protected string ObtainOrderID(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultOrderId,
                "Order Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the order ID" : promptMessage,
                "The Order ID can't be empty");
        }

        /// <summary>
        /// Obtain an cart ID to work with the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message</param>
        /// <returns>The cart ID</returns>
        protected string ObtainCartID(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultCartId,
                "Cart Id",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the cart ID" : promptMessage,
                "The cart ID can't be empty");
        }

        /// <summary>
        /// Obtain a quantity to update order  with the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message</param>
        /// <returns>The quantity to update</returns>
        protected string ObtainQuantity(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultQuantity,
                "Quantity",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the Quantity" : promptMessage,
                "The Quantity can't be empty");
        }

        /// <summary>
        /// Obtain billing cycle type to create the order with the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message</param>
        /// <returns>The quantity to update</returns>
        protected string ObtainBillingCycle(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultBillingCycle,
                "Billing Cycle",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the Billing Cycle" : promptMessage,
                "The Billing Cycle can't be empty");
        }

        /// <summary>
        /// Obtain an Azure Subscription Id for provision status with the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="promptMessage">An optional custom prompt message</param>
        /// <returns>Azure subscription Id</returns>
        protected string ObtainAzureSubscriptionId(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultAzureSubscriptionId,
                "Quantity",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the Azure Subscription Id" : promptMessage,
                "The Azure Subscription Id can't be empty");
        }

        /// <summary>
        /// Obtains a subscription ID to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="customerId">The customer ID who owns the subscription.</param>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>The subscription ID.</returns>
        protected string ObtainSubscriptionId(string customerId, string promptMessage = default(string))
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            var subscriptionId = this.Context.Configuration.Scenario.DefaultSubscriptionId;

            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                // get the customer subscriptions and let the user enter the subscription Id afterwards
                this.Context.ConsoleHelper.StartProgress("Retrieving customer subscriptions");
                var subscriptions = partnerOperations.Customers.ById(customerId).Subscriptions.Get();
                this.Context.ConsoleHelper.StopProgress();
                this.Context.ConsoleHelper.WriteObject(subscriptions, "Customer subscriptions");

                Console.WriteLine();
                subscriptionId = this.Context.ConsoleHelper.ReadNonEmptyString(
                    string.IsNullOrWhiteSpace(promptMessage) ? "Enter the subscription ID" : promptMessage, "Subscription ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found subscription ID: {0} in configuration.", subscriptionId);
            }

            return subscriptionId.Trim();
        }

        /// <summary>
        /// Obtains the product SKU ID by asking the user to enter it after displaying customer subscribed SKUs.
        /// </summary>
        /// <param name="customerId">The customer identifier of the customer that has the subscribed SKUs.</param>
        /// <param name="promptMessage">An optional custom prompt message.</param>
        /// <returns>The product SKU identifier.</returns>
        protected string ObtainProductSkuId(string customerId, string promptMessage = default(string))
        {
            var partnerOperations = this.Context.UserPartnerOperations;
            string productSkuId = string.Empty;

            if (string.IsNullOrWhiteSpace(productSkuId))
            {
                // get the customer subscribed Skus and let the user enter the productSku Id afterwards
                this.Context.ConsoleHelper.StartProgress("Retrieving customer subscribed SKUs");
                var customerSubscribedSkus = partnerOperations.Customers.ById(customerId).SubscribedSkus.Get();
                this.Context.ConsoleHelper.StopProgress();
                this.Context.ConsoleHelper.WriteObject(customerSubscribedSkus, "Customer Subscribed SKUs");

                Console.WriteLine();
                productSkuId = this.Context.ConsoleHelper.ReadNonEmptyString(
                    string.IsNullOrWhiteSpace(promptMessage) ? "Enter the product SKU ID" : promptMessage, "Product SKU ID can't be empty");
            }
            else
            {
                Console.WriteLine("Found product SKU ID: {0} in configuration.", productSkuId);
            }

            return productSkuId.Trim();
        }

        /// <summary>
        /// Runs the scenario logic. This is delegated to the implementing sub class.
        /// </summary>
        protected abstract void RunScenario();

        /// <summary> 
        /// Obtain billing cycle type to work with the configuration if set there or prompts the user to enter it. 
        /// </summary> 
        /// <param name="promptMessage">An optional custom prompt message</param> 
        /// <returns>Billing cycle type</returns> 
        protected string ObtainBillingCycleType(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultBillingCycle,
                "Billing cycle type",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the billing cycle type" : promptMessage,
                "The billing cycle type can't be empty");
        }

        /// <summary> 
        /// Obtain customers' agreements csv file name to work with the configuration if set there or prompts the user to enter it. 
        /// </summary> 
        /// <param name="promptMessage">An optional custom prompt message</param> 
        /// <returns>Billing cycle type</returns> 
        protected string ObtainCustomersAgreementCsvFileName(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultCustomerAgreementCsvFileName,
                "Customer Agreements CSV file name",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the Customer Agreements CSV file name (eg:CustomerAgreements.csv)" : promptMessage,
                "The Customer Agreements CSV file name can't be empty");
        }

        /// <summary> 
        /// Obtain renewal TermDuration to work with from the configuration if set there or prompts the user to enter it. 
        /// </summary> 
        /// <param name="promptMessage">An optional custom prompt message</param> 
        /// <returns>Renewal Term Duration</returns> 
        protected string ObtainRenewalTermDuration(string promptMessage = default(string))
        {
            return this.ObtainValue(
                this.Context.Configuration.Scenario.DefaultRenewalTermDuration,
                "Renewal Term Duration",
                string.IsNullOrWhiteSpace(promptMessage) ? "Enter the renewal Term Duration ID" : promptMessage,
                "The renewal term duration can't be empty");
        }

        /// <summary>
        /// Obtains a value to work with from the configuration if set there or prompts the user to enter it.
        /// </summary>
        /// <param name="configuredValue">The value read from the configuration.</param>
        /// <param name="title">The title of the value.</param>
        /// <param name="promptMessage">The prompt message to use if the value was not set in the configuration.</param>
        /// <param name="errorMessage">The error message to use if the user did not enter a value.</param>
        /// <returns>A string value.</returns>
        private string ObtainValue(string configuredValue, string title, string promptMessage, string errorMessage)
        {
            string value = configuredValue;

            if (string.IsNullOrWhiteSpace(value))
            {
                // The value was not set in the configuration, prompt the user the enter value
                value = this.Context.ConsoleHelper.ReadNonEmptyString(promptMessage, errorMessage);
            }
            else
            {
                Console.WriteLine("Found {0}: {1} in configuration.", title, value);
            }

            return value.Trim();
        }
    }
}