using AutoMapper;
using Core.Entities;
using MediatR;
using static Application.Common.Mappings.MappingProfile;

namespace Application.CodeRequests.Commands.CreateElementOutageCodeRequest;

public class CreateElementOutageCodeRequestCommand : IRequest<List<string>>, IMapFrom<CodeRequest>
{
    public int ElementId { get; set; }
   
    public string? ElementName { get; set; }

    public int? ElementTypeId { get; set; }
    public string? ElementType { get; set; }
    public string? Description { get; set; }
    public DateTime? DesiredExecutionStartTime { get; set; }
    public DateTime? DesiredExecutionEndTime { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRequest, CreateElementOutageCodeRequestCommand>()
            .ReverseMap();
    }
}
