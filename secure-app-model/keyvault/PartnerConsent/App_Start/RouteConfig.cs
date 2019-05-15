// -----------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PartnerConsent
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        /// <summary>
        /// Register ASP.NET routes for the web application
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}