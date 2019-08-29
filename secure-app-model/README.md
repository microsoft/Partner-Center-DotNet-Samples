---
page_type: sample
languages:
- csharp
name: Secure Application Model sample in C#
description: Learn how to use the Secure Application Model with the Partner Center .NET SDK in your apps.
products:
- azure
- ms-graph
---

# Secure App Model

## Overview

Microsoft is introducing a secure, scalable framework for authenticating Cloud Solution Provider (CSP) and Control Panel Vendors (CPV) using multi-factor authentication (MFA). This new model will elevate security for operations involving the Partner Center API. This will help all parties including Microsoft and partners to protect their infrastructure and customer data from security risk.

## Scope

This guide covers the following actors

* Control Panel Vendor (CPV) - A control panel vendor is an independent software vendor that develops applications for use by Cloud Solution Provider partners to integrate with the Partner Center API. A control panel vendor is not a Cloud Solution Provider partner with direct access to the Partner Center Dashboard or the API.
* Direct and indirect Cloud Solution Provider partners who are using app + user authentication to directly integrate with the Partner Center API.

To qualify as a CPV, you must on board to Microsoft Partner Center as a control panel vendor first, more information to come. Please note if you are an existing CSP partner who is also a CPV, this prerequisite applies to you, as well.

## Samples List

| Sample Name | Description |
|-------------|-------------|
| CSP Sample | Sample console application that demonstrates how a Cloud Solution Provider (CSP) partner can utilize the refresh token stored using the Partner Consent sample. |
| Partner Consent | Sample web application developed in C#, that demonstrates how a control panel vendor and Cloud Solution Provider partners can obtain the required consent. This process will store the refresh token for the authenticated user in an instance of Azure Key Vault.|

## Application identity and concepts

### Multi-tenant applications

A multi-tenant application is generally a Software as a Service (SaaS) application. You can configure your application to accept sign-ins from any Azure Active Directory (Azure AD) tenants by configuring the application type as multi-tenant on the Azure dashboard. Users in any Azure AD tenant will be able to sign in to your application after consenting to use their account with your application.  
Please see [How to sign in any Azure Active Directory user using the multi-tenant application pattern](https://docs.microsoft.com/azure/active-directory/develop/howto-convert-app-to-be-multi-tenant) for more information

### Consent framework

For a user to sign in to an application in Azure AD, the application must be represented in the user’s tenant. This allows the organization to do things like apply unique policies when users from their tenant sign in to the application. For a single tenant application, this registration is simple; it’s the one that happens when you register the application in the Azure dashboard.

For a multi-tenant application, the initial registration for the application lives in the Azure AD tenant used by the developer. When a user from a different tenant signs in to the application for the first time, Azure AD asks them to consent to the permissions requested by the application. If they consent, then a representation of the application called a service principal is created in the user’s tenant, and the sign in process can continue. A delegation is also created in the directory that records the user’s consent to the application.

Please note that direct and indirect provider partners who are using app + user authentication and directly integrate with the Partner Center API will have to give consent to their marketplace application using the same consent framework.
