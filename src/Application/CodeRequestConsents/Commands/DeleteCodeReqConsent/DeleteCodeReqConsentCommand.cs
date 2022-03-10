using MediatR;

namespace Application.CodeRequestConsents.Commands.DeleteCodeReqConsent;

public class DeleteCodeReqConsentCommand : IRequest<List<string>>
{
    public int Id { get; set; }
}