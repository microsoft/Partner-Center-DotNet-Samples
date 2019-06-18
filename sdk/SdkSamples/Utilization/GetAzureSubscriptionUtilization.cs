// -----------------------------------------------------------------------
// <copyright file="GetAzureSubscriptionUtilization.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Utilization
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A scenario that shows retrieving utilization records for an Azure subscription.
    /// </summary>
    public class GetAzureSubscriptionUtilization : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAzureSubscriptionUtilization"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public GetAzureSubscriptionUtilization(IScenarioContext context) : base("Retrieve Azure Subscription Utilization Records", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            // get the customer ID and the Azure subscription ID to retrieve its utilization
            string customerId = this.ObtainCustomerId();
            string subscriptionId = this.ObtainSubscriptionId(customerId);

            // retrieve the first 100 utilization records for the last 12 months
            this.Context.ConsoleHelper.StartProgress("Retrieving Azure subscription utilization records");

            Models.ResourceCollection<Models.Utilizations.AzureUtilizationRecord> utilizationRecords = this.Context.UserPartnerOperations.Customers[customerId].Subscriptions[subscriptionId].Utilization.Azure.Query(
                DateTimeOffset.Now.AddYears(-1),
                DateTimeOffset.Now,
                size: 100);

            this.Context.ConsoleHelper.StopProgress();

            // create an Azure utilization enumerator which will aid us in traversing the utilization pages
            Enumerators.IResourceCollectionEnumerator<Models.ResourceCollection<Models.Utilizations.AzureUtilizationRecord>> utilizationRecordEnumerator = this.Context.UserPartnerOperations.Enumerators.Utilization.Azure.Create(utilizationRecords);
            int pageNumber = 1;

            while (utilizationRecordEnumerator.HasValue)
            {
                // print the current utilization results page
                this.Context.ConsoleHelper.WriteObject(
                    utilizationRecordEnumerator.Current,
                    string.Format(CultureInfo.InvariantCulture, "Azure Utilization Records Page: {0}", pageNumber++));

                Console.WriteLine();
                Console.Write("Press any key to retrieve the next Azure utilization records page");
                Console.ReadKey();

                this.Context.ConsoleHelper.StartProgress("Getting next Azure utilization records page");

                // get the next page
                utilizationRecordEnumerator.Next();

                this.Context.ConsoleHelper.StopProgress();
                Console.Clear();
            }
        }
    }
}
