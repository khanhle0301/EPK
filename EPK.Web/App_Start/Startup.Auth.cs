using EPK.Common;
using EPK.Data.Common;
using EPK.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

[assembly: OwinStartup(typeof(Startup))]

namespace EPK.Web
{
    /// <summary>
    ///
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                AllowInsecureHttp = true,
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        /// <summary>
        ///
        /// </summary>
        public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
        {
            //
            public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                context.Validated();
            }

            public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

                if (allowedOrigin == null) allowedOrigin = "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                string userName;
                string password;
                try
                {
                    userName = context.UserName;
                    password = context.Password;
                }
                catch
                {
                    // Could not retrieve the user due to error.
                    context.SetError("server_error", "Lỗi trong quá trình xử lý.");
                    context.Rejected();
                    return;
                }

                var client = new HttpClient
                {
                    BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
                };
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username",userName),
                    new KeyValuePair<string, string>("password", password)
                });

                var result = client.PostAsync("/oauth/token", content).Result;

                string resultContent = result.Content.ReadAsStringAsync().Result;

                JavaScriptSerializer jss = new JavaScriptSerializer();
                var rfidReader = jss.Deserialize<dynamic>(resultContent);
                string token = string.Empty;
                try
                {
                    token = rfidReader["access_token"];
                }
                catch
                {
                    token = string.Empty;
                }

                if (!string.IsNullOrEmpty(token))
                {
                    CommonConstants.Token = token;

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = client.GetAsync("/Api/account/getclaims").Result;

                    var claimsIdentity = response.Content.ReadAsAsync<ClaimsIdentity>().Result;

                    context.Validated(claimsIdentity);
                }
                else
                {
                    context.SetError("invalid_grant", "Tài khoản hoặc mật khẩu không đúng.");
                    context.Rejected();
                }
            }
        }
    }
}