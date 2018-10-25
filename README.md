# Partner Center .NET SDK Samples

![Build status](https://dev.azure.com/partnercenter/sdk/_apis/build/status/partner-center-dotnet-samples-CI)

This console test app provides sample code for all the scenarios supported by the Partner Center .NET SDK. You can also use it for testing.

## Getting Started

Before you build the application, update the values in the *App.config* file to reflect the Azure AD authentication information you created in [Partner Center authentication](https://docs.microsoft.com/partner-center/develop/partner-center-authentication). Specifically, you should use your integration sandbox account settings during early development or for testing in production.

Under **ScenarioSettings** in the *App.config* file, you can set parameters that will be automatically passed into the scenarios that you run.

To modify the list of scenarios that are run, comment out lines in **mainScenarios** or in an individual **Get Scenarios** method found in the *Program.cs* file.

## What to Change

### PartnerServiceSettings

- PartnerServiceApiEndpoint
- AuthenticationAuthorityEndpoint
- GraphEndpoint
- CommonDomain

All thees settings are necessary for the sample API calls to properly function.

### AppAuthentication

The following settings must be modified, so that each scenario functions as excepted

- **ApplicationId**: Your Azure Active Directory application identifier, used for authentication
- **ApplicationSecret**: The application secret, associated with the specified Azure AD application
- **Domain**: The Azure Active Directory domain where the Azure AD application was created

### UserAuthentication

The following settings must be updated, so that each scenario functions as expected

- **ApplicationId**: Your Azure Active Directory application identifier, used for authentication
- **Username**: Your Azure Active Directory username with **AdminAgent** privileges
- **Password**: The password associated with the specified Azure Active Directory user.

Do not change the following configurations

- RedirectUrl
- ResourceUrl

### ScenarioSettings

Do not change the following setting

- **CustomerDomainSuffix**: The domain suffix used when creating a new customer

The following settings can be updated, if left blank information will need to be inputted when running a scenario where it is necessary

- **CustomerIdToDelete**: The ID of the customer used for deletion.
- **DefaultCustomerId**: The customer ID to use in customer-related scenarios.
- **DefaultInvoiceId**: The invoice ID to use in invoice scenarios.
- **PartnerMpnId**: The partner MPN ID to use in indirect partner scenarios.
- **DefaultServiceRequestId**: The service request ID to use in service request scenarios.
- **DefaultSupportTopicId**: The support topic ID to use in service request scenarios.
- **DefaultOfferId**: The offer ID to use in offer scenarios.
- **DefaultOrderId**: The order ID to use in order scenarios.
- **DefaultSubscriptionId**: The subscription ID to use in subscription scenarios.

Each of the following of settings specify the amount of entries per page when retrieving paged content. These settings can be modified if desired

- CustomerPageSize
- DefaultOfferPageSize
- InvoicePageSize
- ServiceRequestPageSize
- SubscriptionPageSize