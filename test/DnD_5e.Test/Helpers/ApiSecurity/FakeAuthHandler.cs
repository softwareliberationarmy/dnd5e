using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
// ReSharper disable ClassNeverInstantiated.Global

namespace DnD_5e.Test.Helpers.ApiSecurity
{
    /// <summary>
    /// Source: https://stackoverflow.com/questions/61769497/skip-jwt-auth-during-tests-asp-net-core-3-1-web-api
    /// </summary>
    public class FakeAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly FakePolicyEvaluator _policyEvaluator;

        public FakeAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, FakePolicyEvaluator policyEvaluator)
            : base(options, logger, encoder, clock)
        {
            _policyEvaluator = policyEvaluator;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var ticket = new AuthenticationTicket(_policyEvaluator.Principal, FakePolicyEvaluator.TestScheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}