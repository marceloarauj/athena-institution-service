using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateAcademicProgramHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CreateAcademicProgramCommand, AthenaApiResponse<AcademicProgramResponseDto>>
    {
        public async Task<AthenaApiResponse<AcademicProgramResponseDto>> Handle(CreateAcademicProgramCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<AcademicProgramResponseDto>.NotFound("Institution not found.");

            var entity = new AcademicProgramEntity
            {
                Name = request.Dto.Name,
                Type = request.Dto.Type,
                PeriodType = request.Dto.PeriodType,
                HasWeeklySchedule = request.Dto.HasWeeklySchedule,
                DurationYears = request.Dto.DurationYears,
                MinCompletionPercent = request.Dto.MinCompletionPercent,
                MinSchoolDays = request.Dto.MinSchoolDays,
                InstitutionId = institution.Id,
                Institution = institution
            };

            await unitOfWork.AcademicProgramRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<AcademicProgramResponseDto>.Created(new AcademicProgramResponseDto(entity));
        }
    }
}
