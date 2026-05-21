using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class UpsertCurriculumEntryHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<UpsertCurriculumEntryCommand, AthenaApiResponse<CurriculumEntryResponseDto>>
    {
        public async Task<AthenaApiResponse<CurriculumEntryResponseDto>> Handle(UpsertCurriculumEntryCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<CurriculumEntryResponseDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<CurriculumEntryResponseDto>.NotFound("Program edition not found.");

            if (IsEditionLocked(edition))
                return AthenaApiResponse<CurriculumEntryResponseDto>.UnprocessableEntity("Cannot modify a published or closed edition.");

            var subject = await unitOfWork.SubjectRepository.FindByIdAsync(request.Dto.SubjectId);
            if (subject == null)
                return AthenaApiResponse<CurriculumEntryResponseDto>.NotFound("Subject not found.");

            var entry = new CurriculumEntryEntity
            {
                ProgramEditionId = edition.Id,
                ProgramEdition = edition,
                SubjectId = subject.Id,
                Subject = subject,
                GradeOrYear = request.Dto.GradeOrYear,
                PeriodNumber = request.Dto.PeriodNumber,
                WeeklyHours = request.Dto.WeeklyHours,
                TotalHours = request.Dto.TotalHours
            };

            await unitOfWork.CurriculumEntryRepository.UpsertAsync(entry);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<CurriculumEntryResponseDto>.Ok(new CurriculumEntryResponseDto(entry));
        }

        private static bool IsEditionLocked(Domain.Entities.ProgramEditionEntity edition) =>
            edition.Status == Domain.Enums.EditionStatus.Published || edition.Status == Domain.Enums.EditionStatus.Closed;
    }
}
