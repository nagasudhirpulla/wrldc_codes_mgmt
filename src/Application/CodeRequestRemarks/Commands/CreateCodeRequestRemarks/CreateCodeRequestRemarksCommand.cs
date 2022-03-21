using AutoMapper;
using Core.Entities;
using MediatR;
using static Application.Common.Mappings.MappingProfile;

namespace Application.CodeRequestRemarks.Commands.CreateCodeRequestRemarks;
public class CreateCodeRequestRemarksCommand :IRequest<List<string>>, IMapFrom<CodeRequestRemark>
{
    public string? StakeholderId { get; set; }
    public int CodeRequestId { get; set; }
    public string? RldcRemarks { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRequestRemark, CreateCodeRequestRemarksCommand>()
            .ReverseMap();
    }
}
