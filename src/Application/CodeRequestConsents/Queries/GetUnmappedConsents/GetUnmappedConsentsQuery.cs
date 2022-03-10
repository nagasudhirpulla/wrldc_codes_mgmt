using Application.Common.Interfaces;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeRequestConsents.Queries.GetUnmappedConsents;

public class GetUnmappedConsentsQuery : IRequest<List<ApplicationUser>>
{
    public GetUnmappedConsentsQuery(int codeReqId)
    {
        CodeReqId = codeReqId;
    }

    public int CodeReqId { get; set; }

    public class GetUnmappedConsentsQueryHandler : IRequestHandler<GetUnmappedConsentsQuery, List<ApplicationUser>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppDbContext _context;

        public GetUnmappedConsentsQueryHandler(UserManager<ApplicationUser> userManager, IAppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<ApplicationUser>> Handle(GetUnmappedConsentsQuery request, CancellationToken cancellationToken)
        {
            // get list of user Ids associated with code request
            List<string?>? mappedUsrIds = await _context.CodeRequestConsents
                                .Where(x => x.CodeRequestId == request.CodeReqId)
                                .Select(x => x.StakeholderId).ToListAsync(cancellationToken: cancellationToken);

            // get the list of users
            List<ApplicationUser> users = await _userManager.Users
                                            .Where(x => !mappedUsrIds.Contains(x.Id))
                                            .ToListAsync(cancellationToken: cancellationToken);

            // get the list of users from reporting software


            return users;
        }
    }
}