using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GeneratePeriodsHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<GeneratePeriodsCommand, AthenaApiResponse<List<ProgramPeriodResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ProgramPeriodResponseDto>>> Handle(GeneratePeriodsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<ProgramPeriodResponseDto>>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<List<ProgramPeriodResponseDto>>.NotFound("Program edition not found.");

            if (edition.Status != EditionStatus.Draft)
                return AthenaApiResponse<List<ProgramPeriodResponseDto>>.UnprocessableEntity("Periods can only be generated for editions in Draft status.");

            var program = edition.AcademicProgram;

            if (program.PeriodType == PeriodType.Module)
                return AthenaApiResponse<List<ProgramPeriodResponseDto>>.BadRequest("Use CreatePeriod for Module type programs.");

            var periods = GeneratePeriods(edition, program);

            await unitOfWork.ProgramPeriodRepository.DeleteByEditionAsync(edition.Id);
            await unitOfWork.ProgramPeriodRepository.AddRangeAsync(periods);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<List<ProgramPeriodResponseDto>>.Ok(periods.Select(p => new ProgramPeriodResponseDto(p)).ToList());
        }

        private static List<ProgramPeriodEntity> GeneratePeriods(ProgramEditionEntity edition, AcademicProgramEntity program)
        {
            var periods = new List<ProgramPeriodEntity>();
            int totalDays = edition.EndDate.DayNumber - edition.StartDate.DayNumber;
            int year = edition.StartDate.Year;

            int periodsPerYear = program.PeriodType switch
            {
                PeriodType.Annual => 1,
                PeriodType.Bimester => 4,
                PeriodType.Trimester => 3,
                PeriodType.Semester => 2,
                _ => 1
            };

            int totalPeriods = program.PeriodType == PeriodType.Annual
                ? 1
                : periodsPerYear * program.DurationYears;

            int daysPerPeriod = totalDays / totalPeriods;
            var currentStart = edition.StartDate;

            for (int i = 0; i < totalPeriods; i++)
            {
                var periodEnd = i == totalPeriods - 1
                    ? edition.EndDate
                    : currentStart.AddDays(daysPerPeriod - 1);

                int periodNumber = i + 1;
                string periodName = program.PeriodType == PeriodType.Annual
                    ? $"Ano Letivo {year}"
                    : $"{GetPeriodLabel(program.PeriodType)} {periodNumber}";

                periods.Add(new ProgramPeriodEntity
                {
                    Number = periodNumber,
                    Name = periodName,
                    StartDate = currentStart,
                    EndDate = periodEnd,
                    ProgramEditionId = edition.Id,
                    ProgramEdition = edition
                });

                currentStart = periodEnd.AddDays(1);
            }

            return periods;
        }

        private static string GetPeriodLabel(PeriodType type) => type switch
        {
            PeriodType.Bimester => "Bimestre",
            PeriodType.Trimester => "Trimestre",
            PeriodType.Semester => "Semestre",
            _ => "Período"
        };
    }
}
