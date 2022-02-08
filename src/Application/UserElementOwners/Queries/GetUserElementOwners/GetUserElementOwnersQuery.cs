using Application.Common.Interfaces;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserElementOwners.Queries.GetUserElementOwners;

public class GetUserElementOwnersQuery : IRequest<List<UserElementOwner>>
{
    public string? UsrId { get; set; }
    public class GetUserElementOwnersQueryHandler : IRequestHandler<GetUserElementOwnersQuery, List<UserElementOwner>>
    {
        private readonly IAppDbContext _context;

        public GetUserElementOwnersQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserElementOwner>> Handle(GetUserElementOwnersQuery request, CancellationToken cancellationToken)
        {
            List<UserElementOwner> res = await _context.UserElementOwners
                                        .Where(x => x.UsrId == request.UsrId)
                                        .ToListAsync(cancellationToken: cancellationToken);
            return res;
        }
    }
}
