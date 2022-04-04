using FluentValidation;

namespace Application.CodeRequests.Commands.EditCodeRequest;

public class EditElementOutageCodeRequestCommandValidator : AbstractValidator<EditElementOutageCodeRequestCommand>
{
    public EditElementOutageCodeRequestCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
