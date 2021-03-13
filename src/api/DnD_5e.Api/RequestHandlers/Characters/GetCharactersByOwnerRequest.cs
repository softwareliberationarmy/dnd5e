using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DnD_5e.Infrastructure.DataAccess;
using DnD_5e.Infrastructure.DataAccess.Pocos;
using MediatR;

namespace DnD_5e.Api.RequestHandlers.Characters
{
    public class GetCharactersByOwnerRequest: IRequest<IEnumerable<CharacterListPoco>>
    {
        private string UserName { get; }

        public GetCharactersByOwnerRequest(string userName)
        {
            UserName = userName;
        }

        // ReSharper disable once ClassNeverInstantiated.Global
        public class Handler: IRequestHandler<GetCharactersByOwnerRequest, IEnumerable<CharacterListPoco>>
        {
            private readonly ICharacterRepository _repository;

            public Handler(ICharacterRepository repository)
            {
                _repository = repository;
            }

            public Task<IEnumerable<CharacterListPoco>> Handle(GetCharactersByOwnerRequest request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_repository.GetByOwner(request.UserName));
            }
        }
    }
}