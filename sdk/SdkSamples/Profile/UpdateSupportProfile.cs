// -----------------------------------------------------------------------
// <copyright file="UpdateSupportProfile.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Profile
{
    using System.Net.Mail;

    /// <summary>
    /// A scenario that updates the partner's support profile.
    /// </summary>
    public class UpdateSupportProfile : BasePartnerScenario
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSupportProfile"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public UpdateSupportProfile(IScenarioContext context) : base("Update partner support profile", context)
        {
        }

        /// <summary>
        /// Executes the scenario.
        /// </summary>
        protected override void RunScenario()
        {
            IAggregatePartner partnerOperations = this.Context.UserPartnerOperations;

            this.Context.ConsoleHelper.StartProgress("Displaying partner support profile");

            Models.Partners.SupportProfile supportProfile = partnerOperations.Profiles.SupportProfile.Get();

            this.Context.ConsoleHelper.WriteObject(supportProfile);
            this.Context.ConsoleHelper.StopProgress();

            string emailId = this.Context.ConsoleHelper.ReadNonEmptyString("Enter the email ID to be updated in support profile", "Email ID cannot be empty");

            if (emailId != new MailAddress(emailId).Address)
            {
                this.Context.ConsoleHelper.Warning("Invalid email ID");
            }
            else
            {
                supportProfile.Email = emailId;
                this.Context.ConsoleHelper.StartProgress("Updating partner support profile");
                Models.Partners.SupportProfile updatedPartnerSupportProfile = partnerOperations.Profiles.SupportProfile.Update(supportProfile);

                this.Context.ConsoleHelper.StopProgress();
                this.Context.ConsoleHelper.WriteObject(updatedPartnerSupportProfile, "Updated partner support profile");
            }
        }
    }
}
