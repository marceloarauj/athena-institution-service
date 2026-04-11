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
    public class DisciplineController(IMediator mediator, InstitutionContext institutionContext) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateDiscipline([FromBody] CreateDisciplineDto dto)
        {
            var response = await mediator.Send(new CreateDisciplineCommand(dto, institutionContext.Alias!));
            return response.AsResult();
        }
    }
}
