using Application.ReportingData.Queries.GetElementForDisplay;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ElementsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ElementsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<object?> GetAsync([FromQuery] string elType = "")
    {
        if (elType.ToLower() == ElementType.Bay.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllBaysQuery());
            return res;
        }
        if (elType.ToLower() == ElementType.Bus.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllBusesQuery());
            return res;
        }
        if (elType.ToLower() == ElementType.BusReactor.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllBusReactorsQuery());
            return res;
        }
        if (elType.ToLower() == ElementType.Compensator.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllCompensatorsQuery());
            return res;
        }
        if (elType.ToLower() == ElementType.Fsc.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllFscsQuery());
            return res;
        }
        if (elType.ToLower() == ElementType.GeneratingUnit.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllGeneratingUnitsQuery());
            return res;
        }
        if (elType.ToLower() == ElementType.HvdcLineCkt.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllHvdcLineCktsQuery());
            return res;
        }
        if (elType.ToLower() == ElementType.HvdcPole.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllHvdcPolesQuery());
            return res;
        }
        if (elType.ToLower() == ElementType.LineReactor.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllLineReactorsQuery());
            return res;
        }
        if (elType.ToLower() == ElementType.Transformer.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllTransformersQuery());
            return res;
        }
        if (elType.ToLower() == ElementType.TransmissionLineCkt.Value.ToLower())
        {
            object res = await _mediator.Send(new GetAllTransmissionLineCktsQuery());                    
            return res;
        }
        return null;
    }
}
