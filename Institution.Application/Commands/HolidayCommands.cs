using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record CreateHolidayCommand(CreateHolidayDto Dto) : IRequestMessage<AthenaApiResponse<HolidayResponseDto>>;
    public record ListHolidaysCommand(int? Year) : IRequestMessage<AthenaApiResponse<List<HolidayResponseDto>>>;
    public record DeleteHolidayCommand(Guid Id) : IRequestMessage<AthenaApiResponse<bool>>;
    public record CreateRecessCommand(CreateRecessDto Dto) : IRequestMessage<AthenaApiResponse<RecessResponseDto>>;
    public record ListRecessesCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<List<RecessResponseDto>>>;
}
