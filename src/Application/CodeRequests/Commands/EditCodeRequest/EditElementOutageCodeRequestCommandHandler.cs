using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.CodeRequests.Commands.EditCodeRequest;

public class EditElementOutageCodeRequestCommandHandler : IRequestHandler<EditElementOutageCodeRequestCommand, List<string>>
{
    private readonly ILogger<EditElementOutageCodeRequestCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;

    public EditElementOutageCodeRequestCommandHandler(ILogger<EditElementOutageCodeRequestCommandHandler> logger, IMapper mapper, IAppDbContext context, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }
    public async Task<List<string>> Handle(EditElementOutageCodeRequestCommand request, CancellationToken cancellationToken)
    {
        List<string> errs = new();
        var codeReq = await _context.CodeRequests
                 .Where(s => s.Id == request.Id)
                 .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (codeReq == null)
        {
            errs.Add("Code Request Id not valid");
            return errs;

        }
        codeReq.Remarks = request.Remarks;
        codeReq.Description = request.Description;
        codeReq.DesiredExecutionStartTime= request.DesiredExecutionStartTime;
        codeReq.DesiredExecutionEndTime= request.DesiredExecutionEndTime;
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.CodeRequests.Any(e => e.Id == request.Id))
            {
                return new List<string>() { $"Code Request Id {request.Id} not present for editing" };
            }
            else
            {
                throw;
            }
        }
        return errs;
    }
}
