using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CodeReqRemarks.Commands.DeleteCodeReqRemarks;
public class DeleteCodeRequestRemarksCommand : IRequest<List<string>>
{
    public int Id { get; set; }
    // TODO complete command and handler
}
