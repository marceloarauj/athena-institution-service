using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ListEvents([FromQuery] ListEventsFilterDto filter)
        {
            var response = await mediator.Send(new ListEventsCommand(filter));
            return response.AsResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            var response = await mediator.Send(new CreateEventCommand(dto));
            return response.AsResult();
        }
    }
}
