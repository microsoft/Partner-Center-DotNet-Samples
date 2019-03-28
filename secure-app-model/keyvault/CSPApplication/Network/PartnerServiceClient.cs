// -----------------------------------------------------------------------
// <copyright file="PartnerServiceClient.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CSPApplication.Network
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Exceptions;
    using Microsoft.Rest;
    using Models;
    using Newtonsoft.Json;

    public sealed class PartnerServiceClient : ServiceClient<PartnerServiceClient>
    {
        /// <summary>
        /// The settings to be used when serializing and de-serializing JSON.
        /// </summary>
        private readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerServiceClient" /> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to be used.</param>
        public PartnerServiceClient(HttpClient httpClient)
            : base(httpClient, false)
        {
            serializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
        }

        /// <summary>
        /// Refreshes the access token using a refresh token.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.></param>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <param name="refreshToken">The refresh token to be used to obtain a new access token.</param>
        /// <param name="clientId">Identifier of the client requesting the token.</param>
        /// <param name="clientSecret">Secret of the client requesting the token.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of <see cref="AuthenticationResult"/> that represents the access token.</returns>
        public async Task<AuthenticationResult> RefreshAccessTokenAsync(string authority, string resource, string refreshToken, string clientId, string clientSecret = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Dictionary<string, string> content;
            HttpResponseMessage response = null;
            AuthenticationResponse authResponse;

            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(authority)))
                {
                    content = new Dictionary<string, string>
                    {
                        ["client_id"] = clientId,
                        ["grant_type"] = "refresh_token",
                        ["refresh_token"] = refreshToken,
                        ["resource"] = resource
                    };

                    if (!string.IsNullOrEmpty(clientSecret))
                    {
                        content.Add("client_secret", clientSecret);
                    }

                    request.Content = new FormUrlEncodedContent(content);

                    response = await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

                    authResponse = await HandleAuthenticationResponseAsync(response).ConfigureAwait(false);

                    return new AuthenticationResult(authResponse);
                }
            }
            finally
            {
                response?.Dispose();
            }
        }

        private async Task<AuthenticationResponse> HandleAuthenticationResponseAsync(HttpResponseMessage response)
        {
            AuthenticationResponse entity;
            string content;

            content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            entity = JsonConvert.DeserializeObject<AuthenticationResponse>(content, serializerSettings);

            if (response.IsSuccessStatusCode)
            {
                return entity;
            }

            if (entity == null)
            {
                throw new AuthenticationException(response.StatusCode.ToString(), response.ReasonPhrase);
            }

            throw new AuthenticationException(entity.Error, entity.ErrorDescription);
        }
    }
}