using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record CreateRoomCommand(CreateRoomDto Dto) : IRequestMessage<AthenaApiResponse<RoomResponseDto>>;
    public record ListRoomsCommand() : IRequestMessage<AthenaApiResponse<List<RoomResponseDto>>>;
}
