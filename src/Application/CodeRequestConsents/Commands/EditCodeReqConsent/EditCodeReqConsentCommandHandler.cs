using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CodeRequestConsents.Commands.EditCodeReqConsent;
public class EditCodeReqConsentCommandHandler : IRequestHandler<EditCodeReqConsentCommand, List<string>>
{
    private readonly ILogger<EditCodeReqConsentCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IAppDbContext _context;

    public EditCodeReqConsentCommandHandler(ILogger<EditCodeReqConsentCommandHandler> logger, IMapper mapper, IAppDbContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }
    public async Task<List<string>> Handle(EditCodeReqConsentCommand request, CancellationToken cancellationToken)
    {
        List<string> errs = new();
        var crconsent = await _context.CodeRequestConsents
                 .Where(s => s.Id == request.Id)
                 .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (crconsent == null)
        {
            errs.Add("Approved Outage Request Id not valid");
            return errs;

        }
        bool isEditRequired = false;
        if (crconsent.Remarks != request.Remarks || crconsent.ApprovalStatus != request.ApprovalStatus)
        {
            isEditRequired = true;
        }
        if (isEditRequired)
        {
            crconsent.Remarks = request.Remarks;
            crconsent.ApprovalStatus = request.ApprovalStatus;

            try
            {

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CodeRequestConsents.Any(e => e.Id == request.Id))
                {
                    return new List<string>() { $"Employee Dept History Id {request.Id} not present for editing" };
                }
                else
                {
                    throw;
                }
            }
            
        }
        return errs;
    }
 }
