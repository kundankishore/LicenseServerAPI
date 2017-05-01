using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using LicenseService.Models;
using LicenseService.Models.BasicAuthentication;

namespace LicenseService.Filters
{
    public class BasicAuthenticationFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    var dnsHost = actionContext.Request.RequestUri.DnsSafeHost;
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    actionContext.Response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", $"realm=\"{dnsHost}\""));
                }
                else
                {
                    string authHeader = null;
                    var auth = actionContext.Request.Headers.Authorization;
                    if (auth != null && auth.Scheme == "Basic")
                    {
                        authHeader = auth.Parameter;
                    }
                    authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));
                    string[] httpRequestHeaderValues = authHeader.Split(':');
                    var username = httpRequestHeaderValues[0];
                    var password = httpRequestHeaderValues[1];

                    var identity = new BasicAuthenticationIdentity(username, password);
                    
                    if (!AreValidCredentials(identity))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    };

                    // Client is authentic, therefore we create a principal here.
                    AuthenticationHelper.SetCurrentPrincipal(actionContext, username, password);
                }
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        private bool AreValidCredentials(BasicAuthenticationIdentity identity)
        {
            string username = ConfigurationManager.AppSettings["Username"];
            string password=  ConfigurationManager.AppSettings["Password"];
            return (identity.Name == username && identity.Password==password);
        }
    }
}