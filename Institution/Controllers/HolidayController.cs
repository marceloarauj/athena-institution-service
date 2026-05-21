using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HolidayController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateHoliday([FromBody] CreateHolidayDto dto)
        {
            var response = await mediator.Send(new CreateHolidayCommand(dto));
            return response.AsResult();
        }

        [HttpGet]
        public async Task<IActionResult> ListHolidays([FromQuery] int? year)
        {
            var response = await mediator.Send(new ListHolidaysCommand(year));
            return response.AsResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteHoliday(Guid id)
        {
            var response = await mediator.Send(new DeleteHolidayCommand(id));
            return response.AsResult();
        }

        [HttpPost("recess")]
        public async Task<IActionResult> CreateRecess([FromBody] CreateRecessDto dto)
        {
            var response = await mediator.Send(new CreateRecessCommand(dto));
            return response.AsResult();
        }

        [HttpGet("recess/{editionId:guid}")]
        public async Task<IActionResult> ListRecesses(Guid editionId)
        {
            var response = await mediator.Send(new ListRecessesCommand(editionId));
            return response.AsResult();
        }
    }
}
