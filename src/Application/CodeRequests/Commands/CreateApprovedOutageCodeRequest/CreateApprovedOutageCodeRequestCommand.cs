using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CodeRequests.Commands.CreateApprovedOutageCodeRequest;

public class CreateApprovedOutageCodeRequestCommand
{
    public string? Description { get; set; }

    public int? ElementId { get; set; }
    public string? ElementName { get; set; }

    public int? ElementTypeId { get; set; }
    public string? ElementType { get; set; }

    public int? OutageTypeId { get; set; }
    public string? OutageType { get; set; }

    public int? OutageTagId { get; set; }
    public string? OutageTag { get; set; }

    public int? OutageAprovalId { get; set; }

    public DateTime? DesiredExecutionStartTime { get; set; }
    public DateTime? DesiredExecutionEndTime { get; set; }
}
