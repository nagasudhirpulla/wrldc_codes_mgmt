using AutoMapper;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Common.Mappings.MappingProfile;

namespace Application.CodeRequests.Commands.EditCodeRequest;
public class EditElementOutageCodeRequestCommand : IRequest<List<string>>, IMapFrom<CodeRequest>
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Remarks { get; set; }
    public DateTime? DesiredExecutionStartTime { get; set; }
    public DateTime? DesiredExecutionEndTime { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeRequest, EditElementOutageCodeRequestCommand>()
            .ReverseMap();
    }
}
