using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateRecessHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CreateRecessCommand, AthenaApiResponse<RecessResponseDto>>
    {
        public async Task<AthenaApiResponse<RecessResponseDto>> Handle(CreateRecessCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<RecessResponseDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<RecessResponseDto>.NotFound("Program edition not found.");

            var entity = new RecessEntity
            {
                Name = request.Dto.Name,
                StartDate = request.Dto.StartDate,
                EndDate = request.Dto.EndDate,
                ProgramEditionId = edition.Id,
                ProgramEdition = edition
            };

            await unitOfWork.HolidayRepository.AddRecessAsync(entity);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<RecessResponseDto>.Created(new RecessResponseDto(entity));
        }
    }
}
