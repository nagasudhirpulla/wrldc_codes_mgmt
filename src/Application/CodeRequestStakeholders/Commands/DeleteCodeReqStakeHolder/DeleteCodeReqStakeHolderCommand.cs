using MediatR;

namespace Application.CodeRequestStakeholders.Commands.DeleteCodeReqStakeHolder;

public class DeleteCodeReqStakeholderCommand : IRequest<List<string>>
{
    public int Id { get; set; }
}
