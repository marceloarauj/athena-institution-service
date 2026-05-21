using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class BulkUpsertCurriculumHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<BulkUpsertCurriculumCommand, AthenaApiResponse<List<CurriculumEntryResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<CurriculumEntryResponseDto>>> Handle(BulkUpsertCurriculumCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<CurriculumEntryResponseDto>>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<List<CurriculumEntryResponseDto>>.NotFound("Program edition not found.");

            if (IsEditionLocked(edition))
                return AthenaApiResponse<List<CurriculumEntryResponseDto>>.UnprocessableEntity("Cannot modify a published or closed edition.");

            var entries = new List<CurriculumEntryEntity>();
            foreach (var dto in request.Dto.Entries)
            {
                var subject = await unitOfWork.SubjectRepository.FindByIdAsync(dto.SubjectId);
                if (subject == null)
                    return AthenaApiResponse<List<CurriculumEntryResponseDto>>.NotFound($"Subject {dto.SubjectId} not found.");

                entries.Add(new CurriculumEntryEntity
                {
                    ProgramEditionId = edition.Id,
                    ProgramEdition = edition,
                    SubjectId = subject.Id,
                    Subject = subject,
                    GradeOrYear = dto.GradeOrYear,
                    PeriodNumber = dto.PeriodNumber,
                    WeeklyHours = dto.WeeklyHours,
                    TotalHours = dto.TotalHours
                });
            }

            await unitOfWork.CurriculumEntryRepository.BulkUpsertAsync(entries);
            await unitOfWork.CommitAsync();

            var result = await unitOfWork.CurriculumEntryRepository.GetByEditionAsync(edition.Id);
            return AthenaApiResponse<List<CurriculumEntryResponseDto>>.Ok(result.Select(e => new CurriculumEntryResponseDto(e)).ToList());
        }

        private static bool IsEditionLocked(ProgramEditionEntity edition) =>
            edition.Status == EditionStatus.Published || edition.Status == EditionStatus.Closed;
    }
}
