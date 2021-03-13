using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.Security;
using MediatR;
using Microsoft.Extensions.Options;

namespace DnD_5e.Api.RequestHandlers
{
    public class GetAuthValuesRequest : IRequest<GetAuthValuesRequest.Response>
    {
        public class Response
        {
            public string Domain { get; set; }
            public string ClientId { get; set; }
            public string Audience { get; set; }
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