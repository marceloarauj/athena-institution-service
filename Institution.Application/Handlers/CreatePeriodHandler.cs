using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreatePeriodHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CreatePeriodCommand, AthenaApiResponse<ProgramPeriodResponseDto>>
    {
        public async Task<AthenaApiResponse<ProgramPeriodResponseDto>> Handle(CreatePeriodCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ProgramPeriodResponseDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<ProgramPeriodResponseDto>.NotFound("Program edition not found.");

            var entity = new ProgramPeriodEntity
            {
                Number = request.Dto.Number,
                Name = request.Dto.Name,
                StartDate = request.Dto.StartDate,
                EndDate = request.Dto.EndDate,
                ProgramEditionId = edition.Id,
                ProgramEdition = edition
            };

            await unitOfWork.ProgramPeriodRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ProgramPeriodResponseDto>.Created(new ProgramPeriodResponseDto(entity));
        }
    }
}
