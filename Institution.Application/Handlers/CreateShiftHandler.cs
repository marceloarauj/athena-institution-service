using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateShiftHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CreateShiftCommand, AthenaApiResponse<ShiftResponseDto>>
    {
        public async Task<AthenaApiResponse<ShiftResponseDto>> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ShiftResponseDto>.NotFound("Institution not found.");

            var entity = new ShiftEntity
            {
                Name = request.Dto.Name,
                StartTime = request.Dto.StartTime,
                EndTime = request.Dto.EndTime,
                InstitutionId = institution.Id,
                Institution = institution
            };

            await unitOfWork.ShiftRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ShiftResponseDto>.Created(new ShiftResponseDto(entity));
        }
    }
}
