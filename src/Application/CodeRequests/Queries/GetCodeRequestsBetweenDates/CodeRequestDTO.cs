using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.ReportingData;
using static Application.Common.Mappings.MappingProfile;

namespace Application.CodeRequests.Queries.GetCodeRequestsBetweenDates;

public class CodeRequestDTO : IMapFrom<CodeRequest>
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public CodeType? CodeType { get; set; }
    public CodeRequestStatus? RequestState { get; set; }
    public string? Code { get; set; }

    public string? Requester { get; set; }
    public string? RequesterId { get; set; }

    public string? Description { get; set; }
    public string? Remarks { get; set; }

    public int? ElementId { get; set; }
    public string? ElementName { get; set; }

    public int? ElementTypeId { get; set; }
    public string? ElementType { get; set; }

    public int? OutageTypeId { get; set; }
    public string? OutageType { get; set; }

    public int? OutageTagId { get; set; }
    public string? OutageTag { get; set; }
    public int? OutageId { get; set; }
    public int? OutageRequestId { get; set; }

    public DateTime? DesiredExecutionStartTime { get; set; }
    public DateTime? DesiredExecutionEndTime { get; set; }

    public List<ReportingOwner> ElementOwners { get; set; } = new();
    public List<(string Id, string DisplayName)> ConcernedStakeholders { get; set; } = new();
    public List<CodeRequestConsent> ConsentRequests { get; set; } = new();
    public List<CodeRequestRemark> RemarksRequests { get; set; } = new();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRequest, CodeRequestDTO>()
            .ForMember(d => d.Requester, opt => opt.MapFrom(s => s.Requester!.DisplayName))
            .ForMember(d => d.ElementOwners, opt => opt.MapFrom(s => s.ElementOwners.Select(x => new ReportingOwner(x.OwnerId, x.OwnerName!)).ToList()))
            .ForMember(d => d.ConcernedStakeholders, opt => opt.MapFrom(s => s.ConcernedStakeholders
                                                                                .Select(x => new ValueTuple<string, string>(x.Stakeholder!.Id, x.Stakeholder!.DisplayName ?? x.Stakeholder!.UserName))
                                                                                .ToList()));
    }
}