// -----------------------------------------------------------------------
// <copyright file="ScenarioContext.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Samples.Context
{
    using System;
    using System.Threading.Tasks;
    using Configuration;
    using Extensions;
    using Helpers;
    using IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Scenario context implementation class.
    /// </summary>
    public class ScenarioContext : IScenarioContext
    {
        /// <summary>
        /// The redirect URI used when performing app + user authentication.
        /// </summary>
        private static readonly Uri redirectUri = new Uri("urn:ietf:wg:oauth:2.0:oob");

        /// <summary>
        /// A lazy reference to an user based partner operations.
        /// </summary>
        private IAggregatePartner userPartnerOperations = null;

        /// <summary>
        /// A lazy reference to an application based partner operations.
        /// </summary>
        private IAggregatePartner appPartnerOperations = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioContext"/> class.
        /// </summary>
        public ScenarioContext()
        {
            PartnerService.Instance.ApiRootUrl = this.Configuration.PartnerService.PartnerServiceApiEndpoint.ToString();
            PartnerService.Instance.ApplicationName = "Partner Center .NET SDK Samples";
        }

        /// <summary>
        /// Gets a partner operations instance which is application based authenticated.
        /// </summary>
        public IAggregatePartner AppPartnerOperations
        {
            get
            {
                if (this.appPartnerOperations == null)
                {
                    this.ConsoleHelper.StartProgress("Authenticating application");

                    IPartnerCredentials appCredentials = PartnerCredentials.Instance.GenerateByApplicationCredentials(
                        this.Configuration.ApplicationAuthentication.ApplicationId,
                        this.Configuration.ApplicationAuthentication.ApplicationSecret,
                        this.Configuration.ApplicationAuthentication.Domain,
                        this.Configuration.PartnerService.AuthenticationAuthorityEndpoint.OriginalString,
                        this.Configuration.PartnerService.GraphEndpoint.OriginalString);

                    this.ConsoleHelper.StopProgress();
                    this.ConsoleHelper.Success("Authenticated!");

                    this.appPartnerOperations = PartnerService.Instance.CreatePartnerOperations(appCredentials);
                }

                return this.appPartnerOperations;
            }
        }

        /// <summary>
        /// Gets a configuration instance.
        /// </summary>
        public ConfigurationManager Configuration => ConfigurationManager.Instance;

        /// <summary>
        /// Gets a console helper instance.
        /// </summary>
        public ConsoleHelper ConsoleHelper => ConsoleHelper.Instance;

        /// <summary>
        /// Gets a partner operations instance which is user based authenticated.
        /// </summary>
        public IAggregatePartner UserPartnerOperations
        {
            get
            {
                if (this.userPartnerOperations == null)
                {
                    this.ConsoleHelper.StartProgress("Authenticating user");

                    AuthenticationResult aadAuthenticationResult = this.LoginUserToAad();

                    // Authenticate by user context with the partner service
                    IPartnerCredentials userCredentials = PartnerCredentials.Instance.GenerateByUserCredentials(
                        this.Configuration.UserAuthentication.ApplicationId,
                        new AuthenticationToken(
                            aadAuthenticationResult.AccessToken,
                            aadAuthenticationResult.ExpiresOn),
                        delegate
                        {
                            // token has expired, re-Login to Azure Active Directory
                            this.ConsoleHelper.StartProgress("Token expired. Re-authenticating user");
                            AuthenticationResult aadToken = this.LoginUserToAad();
                            this.ConsoleHelper.StopProgress();

                            // give the partner SDK the new add token information
                            return Task.FromResult(new AuthenticationToken(aadToken.AccessToken, aadToken.ExpiresOn));
                        });

                    this.ConsoleHelper.StopProgress();
                    this.ConsoleHelper.Success("Authenticated!");

                    this.userPartnerOperations = PartnerService.Instance.CreatePartnerOperations(userCredentials);
                }

                return this.userPartnerOperations;
            }
        }

        /// <summary>
        /// Logs in to AAD as a user and obtains the user authentication token.
        /// </summary>
        /// <returns>The user authentication result.</returns>
        private AuthenticationResult LoginUserToAad()
        {
            UriBuilder addAuthority = new UriBuilder(this.Configuration.PartnerService.AuthenticationAuthorityEndpoint)
            {
                Path = this.Configuration.PartnerService.CommonDomain
            };

            AuthenticationContext authContext = new AuthenticationContext(addAuthority.Uri.AbsoluteUri);

            return Task.Run(() => authContext.AcquireTokenAsync(
                Configuration.UserAuthentication.ResourceUrl.OriginalString,
                Configuration.UserAuthentication.ApplicationId,
                redirectUri,
                new PlatformParameters(PromptBehavior.Always),
                UserIdentifier.AnyUser)).Result;
        }
    }
}