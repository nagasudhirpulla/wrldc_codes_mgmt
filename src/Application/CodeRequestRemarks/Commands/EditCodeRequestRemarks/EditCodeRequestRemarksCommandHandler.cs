using Application.Common.Interfaces;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CodeRequestRemarks.Commands.EditCodeRequestRemarks;

public class EditCodeRequestRemarksCommandHandler : IRequestHandler<EditCodeRequestRemarksCommand, List<string>>
{
    private readonly ILogger<EditCodeRequestRemarksCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IAppDbContext _context;

    public EditCodeRequestRemarksCommandHandler(ILogger<EditCodeRequestRemarksCommandHandler> logger, IMapper mapper, IAppDbContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<string>> Handle(EditCodeRequestRemarksCommand request, CancellationToken cancellationToken)
    {
        
        var crremarks = await _context.CodeRequestRemarks.Where(coderemark => coderemark.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (crremarks == null)
        {
            string errorMsg = $"Code Remark Id {request.Id} not present for editing";
            return new List<string>() { errorMsg };
        }
        if (crremarks.Remarks != request.Remarks )
        {
            crremarks.Remarks = request.Remarks;
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.CodeRequestRemarks.Any(e => e.Id == request.Id))
            {
                return new List<string>() { $"Order Id {request.Id} not present for editing" };
            }
            else
            {
                throw;
            }
        }

        return new List<string>();
       
    }
}
