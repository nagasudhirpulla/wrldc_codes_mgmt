using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CodeRequests.Commands.CreateOutageRevivalCodeRequest;

public class CreateOutageRevivalCodeRequestCommandValidator : AbstractValidator<CreateOutageRevivalCodeRequestCommand>
{
    public CreateOutageRevivalCodeRequestCommandValidator()
    {
        RuleFor(x => x.OutageId).NotEmpty();
    }
}
