using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListRoomsHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<ListRoomsCommand, AthenaApiResponse<List<RoomResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<RoomResponseDto>>> Handle(ListRoomsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<RoomResponseDto>>.NotFound("Institution not found.");

            var rooms = await unitOfWork.RoomRepository.GetByInstitutionAsync(institution.Id);
            return AthenaApiResponse<List<RoomResponseDto>>.Ok(rooms.Select(r => new RoomResponseDto(r)).ToList());
        }
    }
}
