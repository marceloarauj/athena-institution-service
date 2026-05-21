using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateRoomHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CreateRoomCommand, AthenaApiResponse<RoomResponseDto>>
    {
        public async Task<AthenaApiResponse<RoomResponseDto>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<RoomResponseDto>.NotFound("Institution not found.");

            var entity = new RoomEntity
            {
                Name = request.Dto.Name,
                Capacity = request.Dto.Capacity,
                HasLab = request.Dto.HasLab,
                InstitutionId = institution.Id,
                Institution = institution
            };

            await unitOfWork.RoomRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<RoomResponseDto>.Created(new RoomResponseDto(entity));
        }
    }
}
