// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

[assembly: Microsoft.Owin.OwinStartup(typeof(PartnerConsent.Startup))]

namespace PartnerConsent
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}