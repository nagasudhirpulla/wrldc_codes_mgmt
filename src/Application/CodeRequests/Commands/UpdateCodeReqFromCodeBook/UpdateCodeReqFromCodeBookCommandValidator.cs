using FluentValidation;

namespace Application.CodeRequests.Commands.UpdateCodeReqFromCodeBook;

public class UpdateCodeReqFromCodeBookCommandValidator : AbstractValidator<UpdateCodeReqFromCodeBookCommand>
{
    public UpdateCodeReqFromCodeBookCommandValidator()
    {
        RuleFor(x => x.CodeReqId).GreaterThan(0);
    }
}