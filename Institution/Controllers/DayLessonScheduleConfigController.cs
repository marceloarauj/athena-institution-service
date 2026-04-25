using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DayLessonScheduleConfigController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDayLessonScheduleConfigDto dto)
        {
            var response = await mediator.Send(new CreateDayLessonScheduleConfigCommand(dto));
            return response.AsResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDayLessonScheduleConfigDto dto)
        {
            var response = await mediator.Send(new UpdateDayLessonScheduleConfigCommand(id, dto));
            return response.AsResult();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await mediator.Send(new ListDayLessonScheduleConfigsCommand());
            return response.AsResult();
        }
    }
}
