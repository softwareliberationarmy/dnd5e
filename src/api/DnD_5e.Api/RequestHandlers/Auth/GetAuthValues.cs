using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.Security;
using MediatR;
using Microsoft.Extensions.Options;

namespace DnD_5e.Api.RequestHandlers.Auth
{
    public class GetAuthValuesRequest : IRequest<GetAuthValuesRequest.Response>
    {
        public class Response
        {
            public string Domain { get; init; }
            public string ClientId { get; init; }
            public string Audience { get; init; }
        }

        public class Handler : IRequestHandler<GetAuthValuesRequest, Response>
        {
            private readonly Auth0Settings _settings;

            public Handler(IOptions<Auth0Settings> options)
            {
                _settings = options.Value;
            }

            public async Task<Response> Handle(GetAuthValuesRequest request, CancellationToken cancellationToken)
            {
                return new Response
                {
                    Domain = _settings.Domain,
                    ClientId = _settings.ClientId,
                    Audience = _settings.Audience
                };
            }
        }
    }
}