using FluentValidation;

namespace Application.CodeRequests.Commands.CreateApprovedOutageCodeRequest;

public class CreateApprovedOutageCodeRequestCommandValidator : AbstractValidator<CreateApprovedOutageCodeRequestCommand>
{
    public CreateApprovedOutageCodeRequestCommandValidator()
    {
        RuleFor(x => x.OutageApprovalId).NotEmpty();
    }
}
