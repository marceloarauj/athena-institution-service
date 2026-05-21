using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Institution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto dto)
        {
            var response = await mediator.Send(new CreateRoomCommand(dto));
            return response.AsResult();
        }

        [HttpGet]
        public async Task<IActionResult> ListRooms()
        {
            var response = await mediator.Send(new ListRoomsCommand());
            return response.AsResult();
        }
    }
}
