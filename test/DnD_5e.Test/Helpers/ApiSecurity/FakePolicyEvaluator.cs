using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace DnD_5e.Test.Helpers.ApiSecurity
{
    /// <summary>
    /// SOURCE: https://itbackyard.com/how-to-mock-authorize-attribute-for-testing-in-asp-net-core-3-1/
    /// </summary>
    public sealed class FakePolicyEvaluator : IPolicyEvaluator
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

        public async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(Principal,
                new AuthenticationProperties(), TestScheme)));
        }

        public async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
            AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            return await Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }
}
