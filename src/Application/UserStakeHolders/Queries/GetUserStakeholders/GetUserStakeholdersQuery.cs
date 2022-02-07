using Application.Common.Interfaces;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStakeHolders.Queries.GetUserStakeholders;

public class GetUserStakeholdersQuery : IRequest<List<UserStakeholder>>
{
    public string? UsrId { get; set; }
    public class GetUserStakeholdersQueryHandler : IRequestHandler<GetUserStakeholdersQuery, List<UserStakeholder>>
    {
        private readonly IAppDbContext _context;

        public GetUserStakeholdersQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserStakeholder>> Handle(GetUserStakeholdersQuery request, CancellationToken cancellationToken)
        {
            List<UserStakeholder> res = await _context.UserStakeholders
                                        .Where(x => x.UsrId == request.UsrId)
                                        .ToListAsync(cancellationToken: cancellationToken);
            return res;
        }
    }
}
