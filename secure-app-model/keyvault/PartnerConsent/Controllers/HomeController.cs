// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace PartnerConsent.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// Homepage - this is an example of response the consent application can present
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Error page controller to capture exceptions.
        /// This is default implementation and must be tuned to specific experience for an application
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}