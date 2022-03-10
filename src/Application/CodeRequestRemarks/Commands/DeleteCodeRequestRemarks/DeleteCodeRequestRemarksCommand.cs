using MediatR;

namespace Application.CodeRequestRemarks.Commands.DeleteCodeRequestRemarks;
public class DeleteCodeRequestRemarksCommand : IRequest<List<string>>
{
    public int Id { get; set; }
}
