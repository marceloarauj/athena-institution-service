using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetPromotionPreviewHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<GetPromotionPreviewCommand, AthenaApiResponse<PromotionResultDto>>
    {
        public async Task<AthenaApiResponse<PromotionResultDto>> Handle(GetPromotionPreviewCommand request, CancellationToken cancellationToken)
        {
            var result = await ComputePromotion(unitOfWork, request.Dto.SourceEditionId, request.Dto.TargetEditionId, commit: false);
            return result;
        }

        internal static async Task<AthenaApiResponse<PromotionResultDto>> ComputePromotion(
            IUnitOfWork unitOfWork, Guid sourceEditionId, Guid targetEditionId, bool commit)
        {
            var sourceEdition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(sourceEditionId);
            if (sourceEdition == null)
                return AthenaApiResponse<PromotionResultDto>.NotFound("Source edition not found.");

            var targetEdition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(targetEditionId);
            if (targetEdition == null)
                return AthenaApiResponse<PromotionResultDto>.NotFound("Target edition not found.");

            if (targetEdition.Status == Domain.Enums.EditionStatus.Published || targetEdition.Status == Domain.Enums.EditionStatus.Closed)
                return AthenaApiResponse<PromotionResultDto>.UnprocessableEntity("Target edition is already published or closed.");

            var periods = await unitOfWork.ProgramPeriodRepository.GetByEditionAsync(sourceEditionId);
            if (periods.Count == 0)
                return AthenaApiResponse<PromotionResultDto>.BadRequest("Source edition has no periods.");

            var lastPeriod = periods.OrderByDescending(p => p.Number).First();
            var allRecords = await unitOfWork.ProgressRecordRepository.GetByEditionAsync(sourceEditionId);
            var lastPeriodRecords = allRecords.Where(r => r.ProgramPeriodId == lastPeriod.Id).ToList();

            var result = new PromotionResultDto();
            var newEnrollments = new List<Domain.Entities.EnrollmentEntity>();

            foreach (var record in lastPeriodRecords)
            {
                var sourceEnrollment = record.Enrollment;

                switch (record.Status)
                {
                    case ProgressStatus.Promoted:
                        result.TotalPromoted++;
                        newEnrollments.Add(new Domain.Entities.EnrollmentEntity
                        {
                            StudentId = sourceEnrollment.StudentId,
                            ProgramEditionId = targetEditionId,
                            GradeOrYear = sourceEnrollment.GradeOrYear.HasValue ? sourceEnrollment.GradeOrYear + 1 : null,
                            Status = EnrollmentStatus.Active,
                            EnrolledAt = DateTime.UtcNow
                        });
                        break;

                    case ProgressStatus.Retained:
                        result.TotalRetained++;
                        newEnrollments.Add(new Domain.Entities.EnrollmentEntity
                        {
                            StudentId = sourceEnrollment.StudentId,
                            ProgramEditionId = targetEditionId,
                            GradeOrYear = sourceEnrollment.GradeOrYear,
                            Status = EnrollmentStatus.Active,
                            EnrolledAt = DateTime.UtcNow
                        });
                        break;

                    case ProgressStatus.Graduated:
                        result.TotalGraduated++;
                        break;

                    case ProgressStatus.Transferred:
                        result.TotalTransferred++;
                        break;
                }
            }

            if (commit && newEnrollments.Count > 0)
            {
                await unitOfWork.EnrollmentRepository.AddRangeAsync(newEnrollments);
                await unitOfWork.CommitAsync();
            }

            return AthenaApiResponse<PromotionResultDto>.Ok(result);
        }
    }
}
