---
page_type: sample
languages:
- csharp
name: New Commerce Experience Batch Migration Tool (BAM)
description: Learn how to use the New Commerce Experience Batch Migration Tool to migrate your customer's subscription's to NCE.
products:
- azure
- ms-graph
---

# New Commerce Experience Batch Migration Tool (BAM) 

## Table of Contents 

1. [Introduction](#introduction)
2. [Experience Summary](#experience-summary)
3. [Software Pre-requisites](#software-prerequisites)
5. [Step-by-step flow for migrating a batch](#step-by-step-flow-of-migrating-a-batch)
4. [Begin running the Tool and Authenticate Your Account](#begin-running-the-tool-and-authenticate-your-account)
6. [Load .NET console app ](#load--net-console-app)
7. [Export customers](#export-customers)
8. [Export subscriptions for customers with migration eligibility](#export-subscriptions-for-customers-with-migration-eligibility)
9. [Determine which subscriptions will be migrated and how](#determine-which-subscriptions-will-be-migrated-and-how)
10. [Submitting a batch of subscriptions for migration](#submitting-a-batch-of-subscriptions-for-migration)
11. [Checking migration status](#checking-migration-status)
12. [Exporting a list of New Commerce Experience subscriptions](#exporting-a-list-of-new-commerce-experience-subscriptions)
13. [Additional notes and resources](#additional-notes-and-resources)

## Introduction 

To facilitate partner needs of efficiently migrating large quantities of subscriptions before upcoming New Commerce Experience milestones, Microsoft has enabled a Batch Migration (BAM) tool. The BAM tool allows partners to schedule a migration to a future date or can migrate immediately. The BAM tool allows partners to migrate subscriptions into NCE with the following approach:

* Streamlined open source .NET SDK sample app experience 

* Leverages Excel to manage migration edits 

* Simple tool supporting high quality, repeatable, and customizable migration scenarios in batches 

* No code required 

## Experience Summary 

Below are the steps of the console app for migrating immediately or scheduling a migration for the future. 

1. Export customers
2. Export subscriptions with migration eligibility
3. Upload migrations
4. Export migration status
5. Export NCE subscriptions
6. Export subscriptions with migration eligibility to schedule migrations
7. Upload migration schedulers
8. Export scheduled migrations
9. Cancel scheduled migrations
10. Exit

Note that steps six through nine are only for scheduled migrations.

## Prerequisites 
* In order to build and run the BAM tool, .NET 6.0 SDK is required.
* AAD AppId that is onboarded to access Partner Center Apis. The batch migration (BAM) tool is not configured for multitenant apps. When registering the App please use single tenant app.
* Follow the below steps to create an app if not already exists.
	1.	Login to Partner Center and navigate to Account Settings.
	2.	Select App Management and then select “Native App” tab.
	3.	Select “Add new native app” option to onboard / create a new AAD App.
	![Partner Center App Management](assets/images/AccountSettings.png "Partner Center App Management")
	4.	Take note of the newly created App ID.
	5.	Login to Azure Portal and navigate to Active Directory.
	6.	Select “App Registrations” and then select “All Applications” and search for the App ID created in step 3.
	![AppRegistrations](assets/images/AppRegistrations.png "AppRegistrations")
	7.	Select the App from the list and then “Authentication”
	8.	Under “Redirect URIs” ensure the first entry is checked and add a new entry for http://localhost
	![App Authentication](assets/images/AppAuthenticationOptions.png "App Authentication")
	9.	Click “Save”.


## Step-by-step flow of migrating a batch 

### Begin Running the Tool and Authenticate your Account 

**Steps for tool setup**

1. Download the solution from [GitHub](https://github.com/microsoft/Partner-Center-DotNet-Samples/tree/master/nce-bulk-migration-tool)
2. Open command prompt and navigate to the folder where NCEBulkMigrationTool.sln is located.
3. Run command `dotnet build -c Release`
4. Once the application is done building navigate to the folder (NCEBulkMigrationTool\\bin\\Release\\net6.0)
5. Find the file NCEBulkMigrationTool.exe this is the executable which runs the tool.
6. You can either run the tool where you found the original .exe file or you can copy all of the contents of the folder (NCEBulkMigrationTool\\bin\\Release\\net6.0) into a new folder to begin executing the tool.

Steps to run the tool:
1. Using Command Prompt navigate to the folder where is CEBulkMigrationTool.exe located (Use above steps to locate file after building) 
2. In command prompt run the following command.
```
.\\NCEBulkMigrationTool.exe *AppId* *Upn* 
```
**NOTE** If multiple users are running at the same time from the same folder then files can be overwritten or access can be denied. It is better to copy the tool to multiple folders and each user can operate on a separate instance of the tool 
 
### Load .NET console app 

Follow the above section to get the tool running. 

![NCE Batch Migration Tool Running](assets/images/NceBulkMigrationToolLoaded.png "NCE Batch Migration Tool Running")

Once the tool is running and the account is authenticated a partner can perform the above actions using the BAM tool.

## Export customers 

To export a list of customers, enter command “1”. This will produce a CSV similar to the below example. 

![Exported customers CSV](assets/images/ExportedCustomersExample.png "Exported customers CSV")
 
The exported list of customers will be available in the “output” file of the tool’s folders. 

![Input output folder example](assets/images/InputOutputFolderExample.png "Input output folder example")

View exported customers in the file “customers.csv”. For each customer under a partner tenant ID, users can view customer tenant ID, customer domain, and customer company name. 

## Export subscriptions for customers with migration eligibility 

In the downloaded “customers.csv” file, the user can remove rows for the customers whose subscriptions they do not want to export in the next file download. The remaining customers on the file represent customers whose subscriptions will be validated for migration eligibility during the next step in the BAM tool’s flow. 

Please save the updated “customers.csv” in the “input” folder in order to successfully execute the next step of receiving subscriptions for the specified customers. The “input” folder contains 2 other nested folders labelled “migrations” and “subscriptions”. Do not place the “customers.csv” file in the nested folders; simply keep it in the “input” folder. 

Run the BAM tool and enter command “2” to export subscriptions with migration eligibility. 

 ![Export customer subscriptions](assets/images/ExportSubscriptionsExample.png "Export customer subscriptions")

The console app will indicate subscriptions are being validated for eligibility, as seen in the screenshot above. 

 ![Export customer subscriptions in progress](assets/images/ExportSubscriptionsToolOutput.png "Export customer subscriptions in progress")

Once export is complete, the list of subscriptions for the specified customers will be available in the output folder as “subscriptions.csv”. 

 ![Export customer subscriptions result file](assets/images/SubscriptionsOutput.png "Export customer subscriptions result file")

The “subscriptions.csv” file will provide a list of all legacy subscriptions (both active and suspended) under the customers previously specified. 

![Export customer subscriptions result CSV content](assets/images/OutputSubscriptionsResult.png "Export customer subscriptions result CSV content")

The following fields can be viewed for each subscription. 
* Partner tenant ID 
* Indirect Reseller MPN ID 
* Customer Name 
* Customer Tenant ID 
* Legacy Subscription ID 
* Legacy Subscription Name 
* Legacy Product Name 
* Expiration Date 
* Migration Eligible (True or False) 
* Current Term 
* Current Billing Plan 
* Current Seat Count 
* Start New Term (post migration in NCE) 
* Term (post migration in NCE) 
* Billing Plan (post migration in NCE) 
* Seat Count (post migration in NCE) 
* Add On (True or False) 
* Base Subscription (if an add-on) 
* Migration Ineligibility Reason (if subscription is not eligible for migration)
* Custom Term End Date 

## Determine which subscriptions will be migrated and how 

With the fields above, users can filter through the exported list of subscriptions to determine which subscriptions they would like to migrate to NCE in a batch. Supported cases may include filtering to migrate subscriptions of a specific product type or subscriptions under a particular indirect reseller in a batch. 

Once subscriptions have been filtered and selected, please delete subscriptions that are not selected for the batch from the CSV file. This will prevent any unintended migrations. 

**Our recommendation is a limit of 200 subscriptions per batch.** 

The next step is determining how they would like the subscriptions to be migrated (e.g. like-to-like or with updated start new term, billing frequency, term duration or seat count attributes). 

Users may overwrite the following fields in rows for subscriptions they would like to migrate. 
* Start New Term 
* Term 
* Billing Plan 
* Seat Count 
* Customer Term End Date 

These fields represent the instructions or attributes that the NCE subscription will adhere to post-migration. The default values for these fields are the values held by the Legacy subscription being migrated. If no changes are made to a field, the corresponding NCE subscription will have the same value held by the Legacy subscription it migrated from. For example, if a Legacy subscription being migrated has a Current Seat Count of 2 and no changes are made to the Seat Count field, the NCE subscription will have a seat count of 2 post-migration. 

For a subscription to start a new term in NCE, please change the Start New Term flag from FALSE to TRUE. 

**Please do not change values outside of the Start New Term, Term, Billing Plan, and Seat Count columns.**

## Submitting a batch of subscriptions for migration 

Once a batch has been determined (subscriptions for migration have been filtered through and have updated NCE values if desired), save your updated “subscriptions.csv” file in the “subscriptions” folder nested in the “input” folder. Each file saved in the ”subscriptions” folder represents a batch to migrate. 

Once all batch files have been added to the folder run the console app and select option 3, upload migrations, for the app to begin reading in batch files in the “subscriptions” folder and executing migration requests. 

![Upload migrations selection in console](assets/images/UploadMigrationsExample.png "Upload migrations selection in console")

The console will indicate that the migration requests are being processed. 

![Upload migrations tool output](assets/images/UploadMigrationsToolOutput.png "Upload migrations tool output")

Once a file from the “subscriptions” folder has been processed for migration, the tool will move that file into the nested “processed” folder, indicating that migration requests for that batch have been executed. Partners do not need to manually move files into the “processed” folder themselves; files in the “processed” folder will not be read by the app to execute migration on (as they have already been handled). 

![Migrations output folder example](assets/images/MigrationsFolderOutputExample.png "Migrations output folder example")

A file for each batch containing the migration IDs will be exported (available in the “migrations” folder nested in the “output” folder). The exported files will be labelled “[batchId].csv”. 

 ![Uploaded migrations CSV output file example](assets/images/UploadedMigrationsCsvOutput.png "Uploaded migrations CSV output file example")

This file will possess the same fields as the input “subscriptions.csv” file with 2 additional columns labeled Batch ID and Migration ID. The Batch ID will be the same for all subscriptions in the file, indicating these subscriptions belong to the same batch or set of migration requests that were processed together. The Batch ID is also reflected in the name of this csv file. 

## Checking migration status 

The Migration ID is unique to each subscription being migrated. Migration ID can be used to track the migration status. The screenshot above indicates that for all subscriptions, the migration status is still processing (column I). 

Once a migration has finished being executed, the status of the migration will be deemed Completed if migration was successful. The NCE Subscription Id will also be populated upon successful migration. If migration was unsuccessful, migration status will be denoted as Failed and the user will be able to view the error reason. 

To be able to retrieve a refreshed status file for a batch, the exported “[batchId].csv” file (exported to the “migrations” folder nested in “output”) must be copied or saved into the “migrations” folder nested in the “input” folder. This will allow the tool to read in which batches status has been requested for and prepare reports to export. 

Then, a partner must run the console app and select to check migration status. Status files will not be automatically updated. To retrieve updated statuses, a new request must be made each time (see below). 

![Check migration status console example](assets/images/CheckMigrationStatusExample.png "Check migration status console example")

To retrieve updated migration statuses, run the console app and enter command “4”. 

 ![Check migration status console output](assets/images/CheckMigrationStatusToolOutput.png "Check migration status console output")

The console app will indicate migration status is being looked up and that a file has been exported to the "migrationstatus” folder. The names of the exported migration status files represent the batch ID of subscriptions contained in the CSVs. 

 ![Check migration status file output example](assets/images/CheckMigrationStatusFileOutput.png "Check migration status file output example")

Select the “[batchID].csv” file in the "migrationstatus” folder. 

![Check migration status CSV file example](assets/images/CheckMigrationStatusCsvOutput.png "Check migration status CSV file example")

This file will provide updated statuses for migration requests that have been processed. If more than one batch is represented in the file, use the Batch Id column to filter to access statuses of requests in a particular batch. 

## Exporting a list of New Commerce Experience subscriptions 

To export NCE subscriptions, enter command 5. The exported list will show up in your “output” folder and will include the fields displayed in the example file below. 

![Export NCE subscriptions console example](assets/images/NceBulkMigrationToolLoaded.png "Export NCE subscriptions console example")
 
 ![Export NCE subscriptions CSV file output](assets/images/ExportNceSubscriptionsCsvOutput.png "Export NCE subscriptions CSV file output")

 ## Schedule Migration 

Partner can select command `6` to export all the legacy subscriptions with migration eligibility. Once you export the file, you can select the required subscriptions for schedule and modify the configuration as per the schedule.

Partners can update the following fields.

* Start New Term
*Term
* Billing Plan
* Seat Count
*Customer Term End Date
* Target Date or Migrate on Renewal flag

Once the partners prepare the file they can place it in the “subscriptionsforschedule” input folder and ensure not more than 200 subscriptions are in a single batch file. 

Partners can select command `7` to schedule the migration. The output file is generated and placed in “schedulemigrations” output folder. This will file will contain the `ScheduleMigrationID`. If the subscription is not eligible it will have the error code and error reason populated.

Partners can select command `8` to download all the migrations that are scheduled. The output will be copied to the `schedulemigrations.csv` file within the output folder.

Partners can select command `9` to cancel the scheduled run. Take the output file from the previous command and copy it into the `cancelschedulemigrations` input folder. Update the status to **Cancel** in order to cancel the schedule of a particular subscription that has been scheduled for the future.

## Schedule migrations APIs

* Cancel a new commerce migration
*Update a new commerce migration
* Get a new commerce migration
* Schedule a new commerce migration ## Key Scenarios
In the case a user wants to migrate more than 200 subscriptions (the maximum batch size recommendation), multiple batches can be uploaded into the BAM tool. Users can organize folders by a variety of fields to reduce the size of the files they would like to upload to be migrated; users may organize subscriptions to be migrated by indirect reseller, product name, subscription name, and more. If a batch file, which the user has organized, exceeds the maximum recommendation of 200 subscriptions, users may separate one CSV into multiple by effectively copying over subscriptions to new files to maintain the 100 subscription maximum of each batch. For example, if a user would like to migrate 325 subscriptions, this can be split into two files (one with 200 subscriptions and the other with 125 subscriptions). 

Multiple files can be uploaded into the batch tool at once; the tool will read migration requests one batch file and a time and will automatically begin reading in other batch files saved to the input directory (in the case multiple batches have been added). The tool will read in batches one-by-one and call the [Create Migration API](https://docs.microsoft.com/en-us/partner-center/develop/create-migration) on each subscription individually. Users would not need to wait for one batch file to be finished executing to add additional batch files to the input directory. 

The anticipated timelines for each batch to complete is zero to six hours; exceptions will be seen in cases where Partner Center is receiving a high volume of migration requests from multiple users in a short timeframe. More details regarding expected migration timelines are available [here](https://docs.microsoft.com/en-us/partner-center/migrate-subscriptions-to-new-commerce#expected-migration-timelines). 

 ## Known Issues
 * If you receive a 409 error when exporting the subscription list, please wait 5 minutes and retry this step. This error primarily happens in cases where the tool is making parallel calls while the system is still taking action on previous call

## Additional notes and resources 

Please migrate only one batch of subscriptions at a time to avoid API throttling and rate limits. 

Documentation for migrating subscriptions and additional guidelines is available at [Migrate subscriptions to new commerce](/partner-center/migrate-subscriptions-to-new-commerce)
