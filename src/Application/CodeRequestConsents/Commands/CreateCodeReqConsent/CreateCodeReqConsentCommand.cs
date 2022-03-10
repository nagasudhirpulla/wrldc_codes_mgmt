using Core.Entities;
using AutoMapper;
using MediatR;
using static Application.Common.Mappings.MappingProfile;
using System;
using Core.Enums;

namespace Application.CodeRequestConsents.Commands.CreateCodeReqConsent;

public class CreateCodeReqConsentCommand : IRequest<List<string>>, IMapFrom<CodeRequestConsent>
{
    // TODO complete command and hanlder
    public string? StakeholderId { get; set; }
    public int CodeRequestId { get; set; }
    public string? RldcRemarks { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRequestConsent, CreateCodeReqConsentCommand>()
            .ReverseMap();
    }
}