using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DnD_5e.Test.Helpers
{
    /// <summary>
    /// SOURCE: https://itbackyard.com/how-to-mock-authorize-attribute-for-testing-in-asp-net-core-3-1/
    /// </summary>
    public class FakePolicyEvaluator : IPolicyEvaluator
    {
        public const string TestScheme = "FakeScheme";
        public ClaimsPrincipal Principal { get; }

        public FakePolicyEvaluator(string userName)
        {
            Principal = new ClaimsPrincipal();
            Principal.AddIdentity(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, userName)
            }, TestScheme));
        }

        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(Principal,
                new AuthenticationProperties(), TestScheme)));
        }

        public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
            AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            return await Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly FakePolicyEvaluator _policyEvaluator;

        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
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
