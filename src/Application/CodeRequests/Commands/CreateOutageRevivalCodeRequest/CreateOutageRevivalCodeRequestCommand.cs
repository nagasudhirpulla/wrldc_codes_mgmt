using AutoMapper;
using Core.Entities;
using MediatR;
using static Application.Common.Mappings.MappingProfile;

namespace Application.CodeRequests.Commands.CreateOutageRevivalCodeRequest;

public class CreateOutageRevivalCodeRequestCommand : IRequest<List<string>>, IMapFrom<CodeRequest>
{
    public int OutageId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRequest, CreateOutageRevivalCodeRequestCommand>()
            .ReverseMap();
    }
}

