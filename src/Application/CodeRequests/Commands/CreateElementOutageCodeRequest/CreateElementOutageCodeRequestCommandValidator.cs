using FluentValidation;

namespace Application.CodeRequests.Commands.CreateElementOutageCodeRequest;

public class CreateElementOutageCodeRequestCommandValidator : AbstractValidator<CreateElementOutageCodeRequestCommand>
{
    public CreateElementOutageCodeRequestCommandValidator()
    {
        RuleFor(x => x.ElementId).NotEmpty();
    }
}
