// <copyright file="McaHttpClient.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MCARefreshBulkAttestationCLITool
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using MCARefreshBulkAttestationCLITool.Interfaces;
    using Microsoft.Extensions.Logging;

    public static class McaHttpClientExtensions
    {
        public class PartnerCenterAuthorizationHandler : DelegatingHandler
        {
            private readonly ITokenProvider tokenProvider;
            private readonly ILogger<PartnerCenterAuthorizationHandler> logger;

            public PartnerCenterAuthorizationHandler(ITokenProvider tokenProvider)
            {
                this.tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var authenticationToken = await this.tokenProvider.GetTokenAsync();

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticationToken);

                request.Headers.Add("MS-CorrelationId", Guid.NewGuid().ToString());
                request.Headers.Add("MS-RequestId", Guid.NewGuid().ToString());

                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}
