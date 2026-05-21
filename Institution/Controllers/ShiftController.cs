using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateShift([FromBody] CreateShiftDto dto)
        {
            var response = await mediator.Send(new CreateShiftCommand(dto));
            return response.AsResult();
        }

        [HttpGet]
        public async Task<IActionResult> ListShifts()
        {
            var response = await mediator.Send(new ListShiftsCommand());
            return response.AsResult();
        }

        [HttpPost("{shiftId:guid}/slots")]
        public async Task<IActionResult> AddScheduleSlot(Guid shiftId, [FromBody] AddScheduleSlotDto dto)
        {
            dto.ShiftId = shiftId;
            var response = await mediator.Send(new AddScheduleSlotCommand(dto));
            return response.AsResult();
        }

        [HttpGet("{shiftId:guid}/slots")]
        public async Task<IActionResult> ListScheduleSlots(Guid shiftId)
        {
            var response = await mediator.Send(new ListScheduleSlotsCommand(shiftId));
            return response.AsResult();
        }
    }
}
