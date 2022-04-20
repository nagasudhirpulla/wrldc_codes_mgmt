using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CodeRequests.Commands.UpdateCodeReqFromCodeBook;

public class UpdateCodeReqFromCodeBookCommand : IRequest<List<string>>
{
    public int CodeReqId { get; set; }
    public bool IsApproved { get; set; }
    public string? Code { get; set; }
    public DateTime? CodeIssueTime { get; set; }
}
