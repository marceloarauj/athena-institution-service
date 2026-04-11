using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Institution.Infrastructure.Contexts.Models;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController(IMediator mediator, InstitutionContext institutionContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ListEvents([FromQuery] ListEventsFilterDto filter)
        {
            var response = await mediator.Send(new ListEventsCommand(institutionContext.Alias!, filter));
            return response.AsResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            var response = await mediator.Send(new CreateEventCommand(dto, institutionContext.Alias!));
            return response.AsResult();
        }
    }
}
