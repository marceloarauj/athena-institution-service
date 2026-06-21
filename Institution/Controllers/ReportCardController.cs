using AthenaUnionLibrary.ApiResponse;
using AthenaUnionLibrary.Authorization;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/report-card")]
    [Authorize]
    public class ReportCardController(IMediator mediator) : ControllerBase
    {
        [HttpGet("layout")]
        [AthenaPermission("SHOW_SCREEN_SETTINGS")]
        public async Task<IActionResult> GetLayout()
        {
            var response = await mediator.Send(new GetReportCardLayoutCommand());
            return response.AsResult();
        }

        [HttpPost("layout")]
        [AthenaPermission("SHOW_SCREEN_SETTINGS")]
        public async Task<IActionResult> SaveLayout([FromBody] SaveReportCardLayoutDto dto)
        {
            var response = await mediator.Send(new SaveReportCardLayoutCommand(dto));
            return response.AsResult();
        }

        [HttpGet("generate")]
        public async Task<IActionResult> Generate()
        {
            var response = await mediator.Send(new GenerateReportCardCommand());

            if (!response.Success)
                return response.AsResult();

            return File(response.Data!, "application/pdf", "boletim.pdf");
        }
    }
}
