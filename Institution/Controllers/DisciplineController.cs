using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisciplineController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateDiscipline([FromBody] CreateDisciplineDto dto)
        {
            var response = await mediator.Send(new CreateDisciplineCommand(dto));
            return response.AsResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDiscipline(Guid id, [FromBody] UpdateDisciplineDto dto)
        {
            var response = await mediator.Send(new UpdateDisciplineCommand(id, dto));
            return response.AsResult();
        }
    }
}
