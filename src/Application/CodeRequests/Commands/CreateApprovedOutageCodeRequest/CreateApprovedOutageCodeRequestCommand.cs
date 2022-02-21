using AutoMapper;
using Core.Entities;
using MediatR;
using static Application.Common.Mappings.MappingProfile;

namespace Application.CodeRequests.Commands.CreateApprovedOutageCodeRequest;

public class CreateApprovedOutageCodeRequestCommand : IRequest<List<string>>, IMapFrom<CodeRequest>
{
    public int ApprovedOutageRequestId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRequest, CreateApprovedOutageCodeRequestCommand>()
            .ReverseMap();
    }
}
