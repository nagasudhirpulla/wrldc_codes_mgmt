using AutoMapper;
using Core.Entities;
using MediatR;
using static Application.Common.Mappings.MappingProfile;

namespace Application.CodeRequestStakeholders.Commands.CreateCodeReqStakeholder;

public class CreateCodeReqStakeholderCommand : IRequest<List<string>>, IMapFrom<CodeRequestStakeHolder>
{
    public string? StakeholderId { get; set; }
    public int CodeRequestId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRequestStakeHolder, CreateCodeReqStakeholderCommand>()
            .ReverseMap();
    }
}

