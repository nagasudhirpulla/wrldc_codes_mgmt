using FluentValidation;

namespace Application.CodeRequests.Commands.CreateOutageRevivalCodeRequest;

public class CreateOutageRevivalCodeRequestCommandValidator : AbstractValidator<CreateOutageRevivalCodeRequestCommand>
{
    public CreateOutageRevivalCodeRequestCommandValidator()
    {
        RuleFor(x => x.OutageId).NotEmpty();
    }
}
