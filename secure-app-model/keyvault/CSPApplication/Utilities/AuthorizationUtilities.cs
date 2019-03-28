// -----------------------------------------------------------------------
// <copyright file="AuthorizationUtilities.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CSPApplication.Utilities
{
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using Newtonsoft.Json.Linq;

    public static class AuthorizationUtilities
    {
        /// <summary>
        /// Gets AAD token in application only token in non-interactive service principal flow
        /// </summary>
        /// <param name="authority">AAD authority</param>
        /// <param name="audience">Token audience</param>
        /// <param name="clientId">AAD application id </param>
        /// <param name="clientSecret">AAD application secret</param>
        public static async Task<JObject> GetADAppToken(string authority, string audience, string clientId, string clientSecret)
        {
            string loginUrl = string.Format("{0}/oauth2/token", authority);

            WebRequest request = WebRequest.Create(loginUrl);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            string content = string.Format(
                "resource={0}&client_id={1}&client_secret={2}&grant_type=client_credentials",
                HttpUtility.UrlEncode(audience),
                HttpUtility.UrlEncode(clientId),
                HttpUtility.UrlEncode(clientSecret));

            return await GetResponse(request, content);
        }

        /// <summary>
        /// Helper function to execute webrequest and parse response as JObject
        /// </summary>
        /// <param name="request">web request</param>
        /// <param name="content">request content</param>
        /// <returns></returns>
        private static async Task<JObject> GetResponse(WebRequest request, string content)
        {
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(content);
            }

            try
            {
                WebResponse response = await request.GetResponseAsync();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseContent = reader.ReadToEnd();
                    JObject adResponse =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(responseContent);
                    return adResponse;
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    using (StreamReader reader = new StreamReader(webException.Response.GetResponseStream()))
                    {
                        string responseContent = reader.ReadToEnd();
                    }
                }
            }

            return null;
        }
    }
}