using MediatR;

namespace Application.CodeRequests.Commands.DeleteCodeRequest;
public class DeleteCodeRequestCommand : IRequest<List<string>>
{
    public int Id { get; set; }
}
