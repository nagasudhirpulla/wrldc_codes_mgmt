using AutoMapper;
using Core.Entities;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Common.Mappings.MappingProfile;

namespace Application.CodeRequestConsents.Commands.EditCodeReqConsent;
public class EditCodeReqConsentCommand : IRequest<List<string>>, IMapFrom<CodeRequestConsent>
{
    public int Id { get; set; }
    public string? Remarks { get; set; }

    public ApprovalStatus ApprovalStatus { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRequestConsent, EditCodeReqConsentCommand>()
            .ReverseMap();
    }
}
