using FluentValidation;

namespace Application.CodeRequests.Commands.CreateElementOutageCodeRequest;

public class CreateElementOutageCodeRequestCommandValidator : AbstractValidator<CreateElementOutageCodeRequestCommand>
{
    public CreateElementOutageCodeRequestCommandValidator()
    {
        RuleFor(x => x.ElementId).NotEmpty();
        RuleFor(x => x.ElementName).NotEmpty();
        RuleFor(x => x.ElementTypeId).NotEmpty();
        RuleFor(x => x.ElementType).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.DesiredExecutionStartTime).NotEmpty();
        RuleFor(x => x.DesiredExecutionEndTime).NotEmpty();
    }
}
