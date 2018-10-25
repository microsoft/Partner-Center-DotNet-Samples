// -----------------------------------------------------------------------
// <copyright file="ScenarioContext.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
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
            PartnerService.Instance.ApiRootUrl = Configuration.PartnerService.PartnerServiceApiEndpoint.ToString();
            PartnerService.Instance.ApplicationName = "Partner Center .NET SDK Samples";
        }

        /// <summary>
        /// Gets a partner operations instance which is application based authenticated.
        /// </summary>
        public IAggregatePartner AppPartnerOperations
        {
            get
            {
                if (appPartnerOperations == null)
                {
                    ConsoleHelper.StartProgress("Authenticating application");

                    IPartnerCredentials appCredentials = PartnerCredentials.Instance.GenerateByApplicationCredentials(
                    Configuration.ApplicationAuthentication.ApplicationId,
                    Configuration.ApplicationAuthentication.ApplicationSecret,
                    Configuration.ApplicationAuthentication.Domain,
                    Configuration.PartnerService.AuthenticationAuthorityEndpoint.OriginalString,
                    Configuration.PartnerService.GraphEndpoint.OriginalString);

                    ConsoleHelper.StopProgress();
                    ConsoleHelper.Success("Authenticated!");

                    appPartnerOperations = PartnerService.Instance.CreatePartnerOperations(appCredentials);
                }

                return appPartnerOperations;
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
                if (userPartnerOperations == null)
                {
                    ConsoleHelper.StartProgress("Authenticating user");
                    AuthenticationResult aadAuthenticationResult = LoginUserToAad();

                    // Authenticate by user context with the partner service
                    IPartnerCredentials userCredentials = PartnerCredentials.Instance.GenerateByUserCredentials(
                        Configuration.UserAuthentication.ApplicationId,
                        new AuthenticationToken(
                            aadAuthenticationResult.AccessToken,
                            aadAuthenticationResult.ExpiresOn),
                        delegate
                        {
                            // token has expired, re-Login to Azure Active Directory
                            ConsoleHelper.StartProgress("Token expired. Re-authenticating user");
                            AuthenticationResult aadToken = LoginUserToAad();
                            ConsoleHelper.StopProgress();

                            // give the partner SDK the new add token information
                            return Task.FromResult(new AuthenticationToken(aadToken.AccessToken, aadToken.ExpiresOn));
                        });

                    ConsoleHelper.StopProgress();
                    ConsoleHelper.Success("Authenticated!");

                    userPartnerOperations = PartnerService.Instance.CreatePartnerOperations(userCredentials);
                }

                return userPartnerOperations;
            }
        }

        /// <summary>
        /// Logs in to AAD as a user and obtains the user authentication token.
        /// </summary>
        /// <returns>The user authentication result.</returns>
        private AuthenticationResult LoginUserToAad()
        {
            UriBuilder addAuthority = new UriBuilder(Configuration.PartnerService.AuthenticationAuthorityEndpoint)
            {
                Path = Configuration.PartnerService.CommonDomain
            };

            UserPasswordCredential userCredentials = new UserPasswordCredential(
                Configuration.UserAuthentication.UserName,
                Configuration.UserAuthentication.Password);

            AuthenticationContext authContext = new AuthenticationContext(addAuthority.Uri.AbsoluteUri);

            return Task.Run(() => authContext.AcquireTokenAsync(
                Configuration.UserAuthentication.ResourceUrl.OriginalString,
                Configuration.UserAuthentication.ApplicationId,
                userCredentials)).Result;
        }
    }
}