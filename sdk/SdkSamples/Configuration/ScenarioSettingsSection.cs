// -----------------------------------------------------------------------
// <copyright file="ScenarioSettingsSection.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Configuration
{
    /// <summary>
    /// Holds the scenario specific settings section.
    /// </summary>
    public class ScenarioSettingsSection : Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioSettingsSection"/> class.
        /// </summary>
        public ScenarioSettingsSection() : base("ScenarioSettings")
        {
        }

        /// <summary>
        /// Gets the customer domain suffix.
        /// </summary>
        public string CustomerDomainSuffix
        {
            get
            {
                return this.ConfigurationSection["CustomerDomainSuffix"];
            }
        }

        /// <summary>
        /// Gets the ID of the customer to delete from the TIP account.
        /// </summary>
        public string CustomerIdToDelete
        {
            get
            {
                return this.ConfigurationSection["CustomerIdToDelete"];
            }
        }

        /// <summary>
        /// Gets the ID of the customer user to delete.
        /// </summary>
        public string CustomerUserIdToDelete
        {
            get
            {
                return this.ConfigurationSection["CustomerUserIdToDelete"];
            }
        }

        /// <summary>
        /// Gets the ID of the directory role whose details should be read.
        /// </summary>
        public string DefaultDirectoryRoleId
        {
            get
            {
                return this.ConfigurationSection["DefaultDirectoryRoleId"];
            }
        }

        /// <summary>
        /// Gets the ID of the user member whose details should be read.
        /// </summary>
        public string DefaultUserMemberId
        {
            get
            {
                return this.ConfigurationSection["DefaultUserMemberId"];
            }
        }

        /// <summary>
        /// Gets the ID of the customer whose details should be read.
        /// </summary>
        public string DefaultCustomerId
        {
            get
            {
                return this.ConfigurationSection["DefaultCustomerId"];
            }
        }

        /// <summary>
        /// Gets the configured ID of the configuration policy.
        /// </summary>
        public string DefaultConfigurationPolicyId
        {
            get
            {
                return this.ConfigurationSection["DefaultConfigurationPolicyId"];
            }
        }

        /// <summary>
        /// Gets the configured ID of the Devices Batch.
        /// </summary>
        public string DefaultDeviceBatchId
        {
            get
            {
                return this.ConfigurationSection["DefaultDeviceBatchId"];
            }
        }

        /// <summary>
        /// Gets the configured ID of the Device.
        /// </summary>
        public string DefaultDeviceId
        {
            get
            {
                return this.ConfigurationSection["DefaultDeviceId"];
            }
        }

        /// <summary>
        /// Gets the configured ID of the Batch Upload Status Tracking.
        /// </summary>
        public string DefaultBatchUploadStatusTrackingId
        {
            get
            {
                return this.ConfigurationSection["DefaultBatchUploadStatusTrackingId"];
            }
        }

        /// <summary>
        /// Gets the ID of the indirect reseller id whose details should be read.
        /// </summary>
        public string DefaultIndirectResellerId
        {
            get
            {
                return this.ConfigurationSection["DefaultIndirectResellerId"];
            }
        }

        /// <summary>
        /// Gets the ID of the default customer user.
        /// </summary>
        public string DefaultCustomerUserId
        {
            get
            {
                return this.ConfigurationSection["DefaultCustomerUserId"];
            }
        }

        /// <summary>
        /// Gets the number of customers to return in each customer page.
        /// </summary>
        public int CustomerPageSize
        {
            get
            {
                return int.Parse(this.ConfigurationSection["CustomerPageSize"]);
            }
        }

        /// <summary>
        /// Gets the number of customer users to return in each customer user page.
        /// </summary>
        public string CustomerUserPageSize
        {
            get
            {
                return this.ConfigurationSection["CustomerUserPageSize"];
            }
        }

        /// <summary>
        /// Gets the number of offers to return in each offer page.
        /// </summary>
        public int DefaultOfferPageSize
        {
            get
            {
                return int.Parse(this.ConfigurationSection["DefaultOfferPageSize"]);
            }
        }

        /// <summary>
        /// Gets the number of invoices to return in each invoice page.
        /// </summary>
        public int InvoicePageSize
        {
            get
            {
                return int.Parse(this.ConfigurationSection["InvoicePageSize"]);
            }
        }

        /// <summary>
        /// Gets the configured Invoice ID.
        /// </summary>
        public string DefaultInvoiceId
        {
            get
            {
                return this.ConfigurationSection["DefaultInvoiceId"];
            }
        }

        /// <summary>
        /// Gets the configured Receipt ID.
        /// </summary>
        public string DefaultReceiptId
        {
            get
            {
                return this.ConfigurationSection["DefaultReceiptId"];
            }
        }

        /// <summary>
        /// Gets the configured partner MPD ID.
        /// </summary>
        public string PartnerMpnId
        {
            get
            {
                return this.ConfigurationSection["PartnerMpnId"];
            }
        }

        /// <summary>
        /// Gets the configured offer ID.
        /// </summary>
        public string DefaultOfferId
        {
            get
            {
                return this.ConfigurationSection["DefaultOfferId"];
            }
        }

        /// <summary>
        /// Gets the configured product ID.
        /// </summary>
        public string DefaultProductId
        {
            get
            {
                return this.ConfigurationSection["DefaultProductId"];
            }
        }

        /// <summary>
        /// Gets the configured SKU ID.
        /// </summary>
        public string DefaultSkuId
        {
            get
            {
                return this.ConfigurationSection["DefaultSkuId"];
            }
        }

        /// <summary>
        /// Gets the configured availability ID.
        /// </summary>
        public string DefaultAvailabilityId
        {
            get
            {
                return this.ConfigurationSection["DefaultAvailabilityId"];
            }
        }

        /// <summary>
        /// Gets the configured order ID.
        /// </summary>
        public string DefaultOrderId
        {
            get
            {
                return this.ConfigurationSection["DefaultOrderId"];
            }
        }

        /// <summary>
        /// Gets the configured subscription ID.
        /// </summary>
        public string DefaultSubscriptionId
        {
            get
            {
                return this.ConfigurationSection["DefaultSubscriptionId"];
            }
        }

        /// <summary>
        /// Gets the service request ID.
        /// </summary>
        public string DefaultServiceRequestId
        {
            get
            {
                return this.ConfigurationSection["DefaultServiceRequestId"];
            }
        }

        /// <summary>
        /// Gets the number of service requests to return in each service request page.
        /// </summary>
        public int ServiceRequestPageSize
        {
            get
            {
                return int.Parse(this.ConfigurationSection["ServiceRequestPageSize"]);
            }
        }

        /// <summary>
        /// Gets the configured support topic ID for creating new service request.
        /// </summary>
        public string DefaultSupportTopicId
        {
            get
            {
                return this.ConfigurationSection["DefaultSupportTopicId"];
            }
        }

        /// <summary>
        /// Gets the configured agreement template ID for create new customer agreements.
        /// </summary>
        public string DefaultAgreementTemplateId
        {
            get { return this.ConfigurationSection["DefaultAgreementTemplateId"]; }
        }

        /// <summary>
        /// Gets the partner's user ID for creating new customer agreement.
        /// </summary>
        public string DefaultPartnerUserId
        {
            get
            {
                return this.ConfigurationSection["DefaultPartnerUserId"];
            }
        }

        /// <summary>
        /// Gets the cart Id for an existing cart
        /// </summary>
        public string DefaultCartId
        {
            get
            {
                return this.ConfigurationSection["DefaultCartId"];
            }
        }

        /// <summary>
        /// Gets the Quantity for updating an existing cart
        /// </summary>
        public string DefaultQuantity
        {
            get
            {
                return this.ConfigurationSection["DefaultQuantity"];
            }
        }

        /// <summary>
        /// Gets the Catalog Item Id for an item from catalog
        /// </summary>
        public string DefaultCatalogItemId
        {
            get
            {
                return this.ConfigurationSection["DefaultCatalogItemId"];
            }
        }

        /// <summary>
        /// Gets the scope for provisioning status
        /// </summary>
        public string DefaultScope
        {
            get
            {
                return this.ConfigurationSection["DefaultScope"];
            }
        }

        /// <summary>
        /// Gets the Azure Subscription Id for provision status
        /// </summary>
        public string DefaultAzureSubscriptionId
        {
            get
            {
                return this.ConfigurationSection["DefaultAzureSubscriptionId"];
            }
        }

        /// <summary>
        /// Gets the BillingCycle for creating a cart
        /// </summary>
        public string DefaultBillingCycle
        {
            get
            {
                return this.ConfigurationSection["DefaultBillingCycle"];
            }
        }

        /// <summary>
        /// Gets the customer agreements file name.
        /// </summary>
        public string DefaultCustomerAgreementCsvFileName
        {
            get
            {
                return this.ConfigurationSection["DefaultCustomerAgreementCsvFileName"];
            }
        }
        
        
        /// <summary>
        /// Gets the configured Currency code.
        /// </summary>
        public string DefaultCurrencyCode
        {
            get
            {
                return this.ConfigurationSection["DefaultCurrencyCode"];
            }
        }

        /// <summary>
        /// Gets the default renewal term duration.
        /// </summary>
        public string DefaultRenewalTermDuration
        {
            get
            {
                return this.ConfigurationSection["DefaultRenewalTermDuration"];
            }
        }
    }
}
