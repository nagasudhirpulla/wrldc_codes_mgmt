using AutoMapper;
using Core.Entities;
using MediatR;
using static Application.Common.Mappings.MappingProfile;

namespace Application.CodeRequestRemarks.Commands.EditCodeRequestRemarks;

public class EditCodeRequestRemarksCommand : IRequest<List<string>>, IMapFrom<CodeRequestRemark>
{
    public int Id { get; set; }
    public string? Remarks { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRequestRemark, EditCodeRequestRemarksCommand>()
            .ReverseMap();
    }
}
