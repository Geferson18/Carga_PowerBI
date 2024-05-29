using Billycock_MS_Reusable.DTO.Common;
using Billycock_MS_Reusable.DTO.Utils;
using Billycock_MS_Reusable.Models.Utils;
using Billycock_MS_Reusable.Models;
using Billycock_MS_Reusable.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Billycock_MS_Reusable.Repositories.Utils.TokenSwagger
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        #region Property
        public readonly ILoginRepository _loginRepository;
        #endregion

        #region Constructor  
        public BasicAuthenticationHandler(ILoginRepository loginRepository,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _loginRepository = loginRepository;
        }
        #endregion

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string userName;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                userName = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();

                bool response = await _loginRepository.ValidateCredentials(new GeneralClass<object>()
                {
                    objeto = JsonConvert.SerializeObject(new { userName, password }),
                    tipo = "Reusable"
                });

                if (response == false)
                    throw new ArgumentException("Invalid credentials");
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, userName)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
