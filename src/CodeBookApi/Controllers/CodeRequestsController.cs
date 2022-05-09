using Application.CodeRequests.Commands.UpdateCodeReqFromCodeBook;
using Application.CodeRequests.Queries.GetCodeRequestsBetweenDates;
using CodeBookApi.Infra.BasicAuth;
using Core.Enums;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

namespace CodeBookApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CodeRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CodeRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
    [HttpGet("getPendingCodes")]
    public async Task<List<CodeRequestDTO>> GetAsync([FromQuery] string? startDt, [FromQuery] string? endDt)
    {
        string format = "yyyy-MM-dd";
        DateTime startDate = DateTime.ParseExact(startDt ?? "", format, CultureInfo.InvariantCulture);
        DateTime endDate = DateTime.ParseExact(endDt ?? "", format, CultureInfo.InvariantCulture);
        List<CodeRequestDTO> codeReqs = await _mediator.Send(new GetCodeRequestsBetweenDatesQuery() { StartDate = startDate, EndDate = endDate });
        codeReqs = codeReqs.Where(c => c.RequestState != CodeRequestStatus.Approved && c.RequestState != CodeRequestStatus.DisApproved).ToList();

        // convert list of stakeholders in string
        //string str = string.Empty;
        foreach (var item in codeReqs)
        {
            string str = string.Empty;
            str = String.Join(",", item.ConcernedStakeholders);
            
        }
            
        return codeReqs;
    }

    [BasicAuth]
    [HttpPost("updateCode")]
    public async Task<List<string>> UpdateCodeRequestAsync([FromBody] UpdateCodeReqFromCodeBookCommand updateCodeReqCommand)
    {
        ValidationResult validationCheck = new UpdateCodeReqFromCodeBookCommandValidator().Validate(updateCodeReqCommand);
        if (!validationCheck.IsValid)
        {
            return validationCheck.Errors.Select(e => e.ErrorMessage).ToList();
        }
        var errs = await _mediator.Send(updateCodeReqCommand);
        return errs;
    }
}
