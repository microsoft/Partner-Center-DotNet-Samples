---
page_type: sample
languages:
- csharp
name: Partner Center .NET SDK Samples
description: Learn how to use the Partner Center .NET SDK in your apps."
products:
- azure
- ms-graph
---

# Partner Center .NET SDK Samples

## Overview

Console application that demonstrates each scenario the Partner Center .NET SDK is capable of performing.

## Getting Started

Before you build the application, update the values in the *App.config* file to reflect the Azure AD authentication information you created in [Partner Center authentication](https://docs.microsoft.com/partner-center/develop/partner-center-authentication). Specifically, you should use your integration sandbox account settings during early development or for testing in production.

Under **ScenarioSettings** in the *App.config* file, you can set parameters that will be automatically passed into the scenarios that you run.

To modify the list of scenarios that are run, comment out lines in **MainScenarios** or in an individual **Get Scenarios** method found in the *Program.cs* file.

## Prerequisites

With previous versions of this sample there were configurations for user credentials. Those configurations have been removed, and now you will be prompted to enter the credentials upon execution. This change was made due to the upcoming requirement to use multi-factor authentication (MFA) when access Partner Center and the Partner Center API using app + user authentication. Please note that the approach used by this application to obtain user credentials is just one way this operation can be performed. It is recommended that you review the [Secure App Model](../secure-app-model/README.md) sample for more information.

### Azure Active Directory

Perform the following task to correctly configure the Azure AD application for use with this sample project.

1. Sign in to the [Partner Center](https://partner.microsoft.com/cloud-solution-provider/csp-partner) using credentials that have *Admin Agent* and *Global Admin* privileges
2. Click on _Dashboard_  at the top of the page, then click on the cog icon in the upper right, and then click the _Partner settings_.
3. Add a new native application if one does not exist already.
4. Sign in to the [Azure management portal](https://portal.azure.com) using the same credentials from step 1.
5. Click on the _Azure Active Directory_ icon in the toolbar.
6. Click _App registrations_ -> Select _All apps_ from the drop down -> Click on the application created in step 3.
7. Click _Settings_ and then click _Redirect URIs_
8. Add **urn:ietf:wg:oauth:2.0:oob** as one of the available Redirect URIs. Be sure to click the _Save_ button to ensure the changes are saved.  

## What to Change

### PartnerServiceSettings

If you are connecting to one of the sovereign clouds you will need to modify the values below. These values should not be modified if you are connecting to the commercial cloud.

- PartnerServiceApiEndpoint
- AuthenticationAuthorityEndpoint
- GraphEndpoint

All thees settings are necessary for the sample API calls to properly function.

### AppAuthentication

The following settings must be modified, so that each scenario functions as excepted

- **ApplicationId**: Your Azure Active Directory application identifier, used for authentication
- **ApplicationSecret**: The application secret, associated with the specified Azure AD application
- **Domain**: The Azure Active Directory domain where the Azure AD application was created

### UserAuthentication

The following settings must be updated, so that each scenario functions as expected

- **ApplicationId**: Your Azure Active Directory application identifier, used for authentication

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
