using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Api.Security;
using MediatR;
using Microsoft.Extensions.Options;

namespace DnD_5e.Api.RequestHandlers
{
    public class GetAuthValues
    {
        public class Request : IRequest<Response>
        {
        }

        public class Response
        {
            public string Domain { get; set; }
            public string ClientId { get; set; }
            public string Audience { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly Auth0Settings _settings;

            public Handler(IOptions<Auth0Settings> options)
            {
                _settings = options.Value;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
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