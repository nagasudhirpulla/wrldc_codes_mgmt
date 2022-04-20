using MediatR;

namespace Application.CodeRequests.Commands.UpdateCodeReqFromCodeBook;

public class UpdateCodeReqFromCodeBookCommandHandler : IRequestHandler<UpdateCodeReqFromCodeBookCommand, List<string>>
{
    public UpdateCodeReqFromCodeBookCommandHandler()
    {
    }
    public async Task<List<string>> Handle(UpdateCodeReqFromCodeBookCommand request, CancellationToken cancellationToken)
    {
        // TODO complete this
        List<string> errs = new();
        return await Task.FromResult(errs);
    }
}