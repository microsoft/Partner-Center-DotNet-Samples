// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples
{
    using Agreements;
    using Analytics;
    using Carts;
    using Context;
    using CustomerDirectoryRoles;
    using CustomerProducts;
    using Customers;
    using CustomerServiceCosts;
    using CustomerSubscribedSkus;
    using CustomerUser;
    using DevicesDeployment;
    using Entitlements;
    using IndirectModel;
    using Invoice;
    using Models.Auditing;
    using Models.Customers;
    using Models.Query;
    using Offers;
    using Orders;
    using Products;
    using Profile;
    using RateCards;
    using RatedUsage;
    using ServiceIncidents;
    using ServiceRequests;
    using Subscriptions;
    using Utilization;
    using Validations;

    /// <summary>
    /// The main program class for the partner center .NET SDK samples.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry function.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            var context = new ScenarioContext();

            var mainScenarios = new[]
            {
                Program.GetCustomerScenarios(context),
                Program.GetAgreementsScenarios(context),
                Program.GetOfferScenarios(context),
                Program.GetProductScenarios(context),
                Program.GetCustomerProductsScenarios(context),
                Program.GetOrderScenarios(context),
                Program.GetSubscriptionScenarios(context),
                Program.GetRatedUsageScenarios(context),
                Program.GetServiceRequestsScenarios(context),
                Program.GetInvoiceScenarios(context),
                Program.GetProfileScenarios(context),
                Program.GetCustomerUserScenarios(context),
                Program.GetCustomerSubscribedSkus(context),
                Program.GetCustomerDirectoryRolesScenarios(context),
                Program.GetAuditingScenarios(context),
                Program.GetRateCardScenarios(context),
                Program.GetSharedRateCardScenarios(context),
                Program.GetIndirectModelScenarios(context),
                Program.GetServiceIncidentScenarios(context),
                Program.GetUtilizationScenarios(context),
                Program.GetPartnerAnalyticsScenarios(context),
                Program.GetCustomerServiceCostsScenarios(context),
                Program.GetAddressValidationsScenarios(context),
                Program.GetDevicesScenarios(context),
                Program.GetCartScenarios(context),
                Program.GetCartWithAddonItemsScenarios(context),
                Program.GetEntitlementScenarios(context)
            };

            // run the main scenario
            new AggregatePartnerScenario("Partner SDK samples", mainScenarios, context).Run();
        }

        /// <summary>
        /// Gets the Customer Directory Roles scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The Customer Directory Roles scenarios.</returns>
        private static IPartnerScenario GetCustomerDirectoryRolesScenarios(ScenarioContext context)
        {
            var customerDirectoryRolesScenarios = new IPartnerScenario[]
            {
                new GetCustomerDirectoryRoles(context),
                new AddUserMemberToDirectoryRole(context),
                new GetCustomerDirectoryRoleUserMembers(context),
                new RemoveCustomerUserMemberFromDirectoryRole(context)
            };

            return new AggregatePartnerScenario("Customer Directory Roles", customerDirectoryRolesScenarios, context);
        }

        /// <summary>
        /// Gets the Customer Subscribed SKUs scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The Customer Subscribed SKUs scenarios.</returns>
        private static IPartnerScenario GetCustomerSubscribedSkus(ScenarioContext context)
        {
            var customerSubscribedSkusScenarios = new IPartnerScenario[]
            {
                new GetCustomerSubscribedSkus(context)
            };

            return new AggregatePartnerScenario("Customer Subscribed Skus", customerSubscribedSkusScenarios, context);
        }

        /// <summary>
        /// Gets the Devices scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The Devices scenarios.</returns>
        private static IPartnerScenario GetDevicesScenarios(ScenarioContext context)
        {
            var devicesScenarios = new IPartnerScenario[]
            {
                new CreateConfigurationPolicy(context),
                new GetAllConfigurationPolicies(context),
                new UpdateConfigurationPolicy(context),
                new DeleteConfigurationPolicy(context),
                new CreateDeviceBatch(context),
                new GetDevicesBatches(context),
                new CreateDevices(context),
                new GetDevices(context),
                new UpdateDevicesPolicy(context),
                new DeleteDevice(context),
                new GetBatchUploadStatus(context)
            };

            return new AggregatePartnerScenario("Devices", devicesScenarios, context);
        }

        /// <summary>
        /// Gets the agreements scenario.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The agreements scenarios.</returns>
        private static IPartnerScenario GetAgreementsScenarios(ScenarioContext context)
        {
            var agreementsScenario = new IPartnerScenario[]
            {
                new GetAgreementDetails(context),
                new GetAgreementDocument(context),
                new GetCustomerAgreements(context),
                new GetAllCustomersAgreements(context),
                new CreateCustomerAgreement(context),
                new ImportCustomersAgreement(context)
            };

            return new AggregatePartnerScenario("Agreements", agreementsScenario, context);
        }

        /// <summary>
        /// Gets the Entitlement scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The Entitlement scenarios.</returns>
        private static IPartnerScenario GetEntitlementScenarios(ScenarioContext context)
        {
            var entitlementScenarios = new IPartnerScenario[]
            {
                new GetEntitlements(context),
            };

            return new AggregatePartnerScenario("Entitlements", entitlementScenarios, context);
        }

        /// <summary>
        /// Gets the customer user scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The customer user scenarios.</returns>
        private static IPartnerScenario GetCustomerUserScenarios(ScenarioContext context)
        {
            var sortScenarios = new IPartnerScenario[]
            {
                new SortCustomerUsers("Ascending order", SortDirection.Ascending, context),
                new SortCustomerUsers("Descending order", SortDirection.Descending, context)
            };

            var customerUserScenarios = new IPartnerScenario[]
            {
                new GetCustomerUserCollection(context),
                new AggregatePartnerScenario("Get sorted customer users", sortScenarios, context),
                new CreateCustomerUser(context),
                new DeleteCustomerUser(context),
                new GetCustomerUserDetails(context),
                new UpdateCustomerUser(context),
                new GetPagedCustomerUsers(context),
                new GetCustomerUserDirectoryRoles(context),
                new CustomerUserAssignedLicenses(context),
                new CustomerUserAssignedGroup1AndGroup2Licenses(context),
                new CustomerUserAssignedGroup1Licenses(context),
                new CustomerUserAssignedGroup2Licenses(context),
                new CustomerUserAssignLicenses(context),
                new CustomerUserAssignGroup1Licenses(context),
                new CustomerUserAssignGroup2Licenses(context),
                new GetCustomerInactiveUsers(context),
                new CustomerUserRestore(context)
            };

            return new AggregatePartnerScenario("Customer User samples", customerUserScenarios, context);
        }

        /// <summary>
        /// Gets the customer scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The customer scenarios.</returns>
        private static IPartnerScenario GetCustomerScenarios(IScenarioContext context)
        {
            var customerFilteringScenarios = new IPartnerScenario[]
            {
                new FilterCustomers("Filter by company name", CustomerSearchField.CompanyName, context),
                new FilterCustomers("Filter by domain name", CustomerSearchField.Domain, context)
            };

            var customerScenarios = new IPartnerScenario[]
            {
                new CreateCustomer(context),
                new CheckDomainAvailability(context),
                new GetPagedCustomers(context, context.Configuration.Scenario.CustomerPageSize),
                new AggregatePartnerScenario("Customer filtering", customerFilteringScenarios, context),
                new GetCustomerDetails(context),
                new GetCustomerQualification(context),
                new UpdateCustomerQualification(context),
                new UpdateCustomerQualificationWithGCC(context),
                new DeleteCustomerFromTipAccount(context),
                new GetCustomerManagedServices(context),
                new GetCustomerRelationshipRequest(context),
                new UpdateCustomerBillingProfile(context),
                new ValidateCustomerAddress(context),
                new DeletePartnerCustomerRelationship(context)
            };

            return new AggregatePartnerScenario("Customer samples", customerScenarios, context);
        }

        /// <summary>
        /// Gets the offer scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The offer scenarios.</returns>
        private static IPartnerScenario GetOfferScenarios(IScenarioContext context)
        {
            var offerScenarios = new IPartnerScenario[]
            {
                new GetOffer(context),
                new GetOfferCategories(context),
                new GetOffers(context),
                new GetPagedOffers(context, context.Configuration.Scenario.DefaultOfferPageSize),
                new GetCustomerOffers(context),
                new GetCustomerOfferCategories(context)
            };

            return new AggregatePartnerScenario("Offer samples", offerScenarios, context);
        }

        /// <summary>
        /// Gets the product scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The product scenarios.</returns>
        private static IPartnerScenario GetProductScenarios(IScenarioContext context)
        {
            var productScenarios = new IPartnerScenario[]
            {
                new GetProducts(context),
                new GetProductsByTargetSegment(context),
                new GetProduct(context),
                new GetSkus(context),
                new GetSkusByTargetSegment(context),
                new GetSku(context),
                new GetAvailabilities(context),
                new GetAvailabilitiesByTargetSegment(context),
                new GetAvailability(context),
                new CheckInventory(context)
            };

            return new AggregatePartnerScenario("Product samples", productScenarios, context);
        }

        /// <summary>
        /// Gets the customer products scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The products for customer scenarios.</returns>
        private static IPartnerScenario GetCustomerProductsScenarios(IScenarioContext context)
        {
            var customerProductsScenarios = new IPartnerScenario[]
            {
                new GetCustomerProducts(context),
                new GetCustomerProductsByTargetSegment(context),
                new GetCustomerProduct(context),
                new GetCustomerSkus(context),
                new GetCustomerSkusByTargetSegment(context),
                new GetCustomerSku(context),
                new GetCustomerAvailabilities(context),
                new GetCustomerAvailabilitiesByTargetSegment(context),
                new GetCustomerAvailability(context)
            };

            return new AggregatePartnerScenario("Products for customers samples", customerProductsScenarios, context);
        }

        /// <summary>
        /// Gets the order scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The order scenarios.</returns>
        private static IPartnerScenario GetOrderScenarios(IScenarioContext context)
        {
            var orderScenarios = new IPartnerScenario[]
            {
                new CreateOrder(context),
                new GetOrderDetails(context),
                new GetOrders(context),
                new CreateAzureReservationOrder(context),
                new GetOrdersByBillingCycleType(context),
                new GetOrderProvisioningStatus(context),
                new UpdateOrder(context),
                new GetLineItemActivationLink(context)
            };

            return new AggregatePartnerScenario("Order samples", orderScenarios, context);
        }

        /// <summary>
        /// Gets the subscription scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The subscription scenarios.</returns>
        private static IPartnerScenario GetSubscriptionScenarios(IScenarioContext context)
        {
            var subscriptionScenarios = new IPartnerScenario[]
            {
                new GetSubscription(context),
                new GetSubscriptions(context),
                new GetSubscriptionsByOrder(context),
                new GetSubscriptionSupportContact(context),
                new UpdateSubscriptionSupportContact(context),
                new GetSubscriptionProvisioningStatus(context),
                new UpdateSubscription(context),
                new UpgradeSubscription(context),
                new AddSubscriptionAddOn(context),
                new ConvertTrialSubscription(context),
                new CancelSaaSSubscription(context),
                new ToggleSubscriptionAutoRenew(context),
                new ActivateSandboxThirdPartySubscription(context),
            };

            return new AggregatePartnerScenario("Subscription samples", subscriptionScenarios, context);
        }

        /// <summary>
        /// Gets the rated usage scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The rated usage scenarios.</returns>
        private static IPartnerScenario GetRatedUsageScenarios(IScenarioContext context)
        {
            var ratedUsageScenarios = new IPartnerScenario[]
            {
                new GetCustomerUsageSummary(context),
                new GetCustomerSubscriptionsUsage(context),
                new GetSubscriptionResourceUsage(context),
                new GetSubscriptionUsageRecords(context),
                new GetSubscriptionUsageSummary(context)
            };

            return new AggregatePartnerScenario("Rated usage samples", ratedUsageScenarios, context);
        }

        /// <summary>
        /// Gets the service request scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The service request scenarios.</returns>
        private static IPartnerScenario GetServiceRequestsScenarios(IScenarioContext context)
        {
            var serviceRequestsScenarios = new IPartnerScenario[]
            {
                new CreatePartnerServiceRequest(context),
                new GetCustomerServiceRequests(context),
                new GetPagedPartnerServiceRequests(context, context.Configuration.Scenario.ServiceRequestPageSize),
                new GetPartnerServiceRequestDetails(context),
                new GetServiceRequestSupportTopics(context),
                new UpdatePartnerServiceRequest(context)
            };

            return new AggregatePartnerScenario("Service request samples", serviceRequestsScenarios, context);
        }

        /// <summary>
        /// Gets the invoice scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The invoice scenarios.</returns>
        private static IPartnerScenario GetInvoiceScenarios(IScenarioContext context)
        {
            var invoiceScenarios = new IPartnerScenario[]
            {
                new GetAccountBalance(context),
                new GetInvoice(context),
                new GetInvoiceLineItems(context, context.Configuration.Scenario.InvoicePageSize),
                new GetPagedInvoices(context, context.Configuration.Scenario.InvoicePageSize),
                new GetInvoiceSummaries(context),
                new GetInvoiceStatement(context),
                new GetInvoiceTaxReceiptStatement(context),
                new GetEstimatesLinks(context),
                new GetBillingLineItemsForOpenPeriodPaging(context, context.Configuration.Scenario.InvoicePageSize),
                new GetUsageLineItemsForOpenPeriodPaging(context, context.Configuration.Scenario.InvoicePageSize),
                new GetUsageLineItemsForClosePeriodPaging(context, context.Configuration.Scenario.InvoicePageSize)
            };

            return new AggregatePartnerScenario("Invoice samples", invoiceScenarios, context);
        }

        /// <summary>
        /// Gets the profile scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The invoice scenarios.</returns>
        private static IPartnerScenario GetProfileScenarios(IScenarioContext context)
        {
            var profileScenarios = new IPartnerScenario[]
            {
                new GetBillingProfile(context),
                new GetLegalBusinessProfile(context),
                new GetOrganizationProfile(context),
                new GetMPNProfile(context),
                new GetSupportProfile(context),
                new UpdateBillingProfile(context),
                new UpdateLegalBusinessProfile(context),
                new UpdateOrganizationProfile(context),
                new UpdateSupportProfile(context)
            };

            return new AggregatePartnerScenario("Partner profile samples", profileScenarios, context);
        }

        /// <summary>
        /// Gets the auditing scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The auditing scenarios.</returns>
        private static IPartnerScenario GetAuditingScenarios(IScenarioContext context)
        {
            var profileScenarios = new IPartnerScenario[]
            {
                new QueryAuditRecords(context),
                new SearchAuditRecords("Filter by company name", AuditRecordSearchField.CompanyName, context),
                new SearchAuditRecordsByCustomerId("Filter by Customer Id", AuditRecordSearchField.CustomerId, context),
                new SearchAuditRecordsByResourceType("Filter by Resource Type (Eg. Order, Customer, or Subscription)", AuditRecordSearchField.ResourceType, context)
            };

            return new AggregatePartnerScenario("Auditing samples", profileScenarios, context);
        }

        /// <summary>
        /// Gets the rate card scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The rate card scenarios.</returns>
        private static IPartnerScenario GetRateCardScenarios(ScenarioContext context)
        {
            return new AggregatePartnerScenario("Rate card samples", new[] { new GetAzureRateCard(context) }, context);
        }

        /// <summary>
        /// Gets the Azure shared services rate card scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>Azure shared services rate card scenarios.</returns>
        private static IPartnerScenario GetSharedRateCardScenarios(ScenarioContext context)
        {
            return new AggregatePartnerScenario("Azure Shared Services Rate card samples", new[] { new GetAzureSharedRateCard(context) }, context);
        }

        /// <summary>
        /// Gets the indirect model scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The invoice scenarios.</returns>
        private static IPartnerScenario GetIndirectModelScenarios(IScenarioContext context)
        {
            var indirectModelScenarios = new IPartnerScenario[]
            {
                new GetIndirectResellers(context),
                new CreateCustomerForIndirectReseller(context),
                new GetIndirectResellersOfCustomer(context),
                new PlaceOrderForCustomer(context),
                new GetCustomersOfIndirectReseller(context),
                new VerifyPartnerMpnId(context),
                new GetSubscriptionsByMpnId(context)
            };

            return new AggregatePartnerScenario("Indirect model samples", indirectModelScenarios, context);
        }

        /// <summary>
        /// Gets the service incident scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The service incident scenarios.</returns>
        private static IPartnerScenario GetServiceIncidentScenarios(ScenarioContext context)
        {
            return new AggregatePartnerScenario("Service incident samples", new[] { new GetServiceIncidents(context) }, context);
        }

        /// <summary>
        /// Gets the Utilization scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The Utilization scenarios.</returns>
        private static IPartnerScenario GetUtilizationScenarios(ScenarioContext context)
        {
            return new AggregatePartnerScenario("Utilization samples", new[] { new GetAzureSubscriptionUtilization(context) }, context);
        }

        /// <summary>
        /// Gets the partner analytics scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The Partner Analytics scenarios.</returns>
        private static IPartnerScenario GetPartnerAnalyticsScenarios(IScenarioContext context)
        {
            var partnerAnalyticsScenarios = new IPartnerScenario[]
            {
                new GetPartnerLicensesDeploymentAnalytics(context),
                new GetPartnerLicensesUsageAnalytics(context),
                new GetCustomerLicensesDeploymentAnalytics(context),
                new GetCustomerLicensesUsageAnalytics(context)
            };

            return new AggregatePartnerScenario("Partner Analytics samples", partnerAnalyticsScenarios, context);
        }

        /// <summary>
        /// Gets the customer service costs scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The customer service costs scenarios.</returns>
        private static IPartnerScenario GetCustomerServiceCostsScenarios(IScenarioContext context)
        {
            var customerServiceCostsScenarios = new IPartnerScenario[]
            {
                new GetCustomerServiceCostsSummary(context),
                new GetCustomerServiceCostsLineItems(context),
            };

            return new AggregatePartnerScenario("Customer service costs samples", customerServiceCostsScenarios, context);
        }

        /// <summary>
        /// Gets the address validation scenarios.
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The address validation scenarios.</returns>
        private static IPartnerScenario GetAddressValidationsScenarios(IScenarioContext context)
        {
            var addressValidationScenarios = new IPartnerScenario[]
            {
                new AddressValidation(context)
            };

            return new AggregatePartnerScenario("Address validation samples", addressValidationScenarios, context);
        }

        /// <summary>
        /// Gets the cart scenarios of create, update and checkout
        /// </summary>
        /// <param name="context">A scenario context.</param>
        /// <returns>The cart scenarios.</returns>
        private static IPartnerScenario GetCartScenarios(IScenarioContext context)
        {
            var cartScenarios = new IPartnerScenario[]
            {
                new CreateCart(context),
                new UpdateCart(context),
                new CheckoutCart(context)
            };

            return new AggregatePartnerScenario("Cart Scenarios", cartScenarios, context);
        }

        /// <summary>
        /// Gets the cart with add on items scenarios of create and checkout
        /// </summary>
        /// <param name="context">A scenario context</param>
        /// <returns>The cart with add on items scenarios.</returns>
        private static IPartnerScenario GetCartWithAddonItemsScenarios(IScenarioContext context)
        {
            var cartScenarios = new IPartnerScenario[]
            {
                new CreateCartWithAddons(context),
                new CheckoutCart(context),
                new CreateCartAddonWithExistingSubscription(context)
            };

            return new AggregatePartnerScenario("Cart With Addon Items Scenarios", cartScenarios, context);
        }
    }
}
