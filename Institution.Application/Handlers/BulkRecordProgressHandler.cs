using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class BulkRecordProgressHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<BulkRecordProgressCommand, AthenaApiResponse<List<ProgressRecordResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ProgressRecordResponseDto>>> Handle(BulkRecordProgressCommand request, CancellationToken cancellationToken)
        {
            var results = new List<ProgressRecordEntity>();

            foreach (var item in request.Dto.Records)
            {
                var existing = await unitOfWork.ProgressRecordRepository.FindByEnrollmentAndPeriodAsync(
                    item.EnrollmentId, request.Dto.ProgramPeriodId);

                if (existing != null)
                {
                    existing.Status = item.Status;
                    existing.FinalGrade = item.FinalGrade;
                    existing.CompletionPercent = item.CompletionPercent;
                    await unitOfWork.ProgressRecordRepository.UpdateAsync(existing);
                    results.Add(existing);
                }
                else
                {
                    var record = new ProgressRecordEntity
                    {
                        EnrollmentId = item.EnrollmentId,
                        ProgramPeriodId = request.Dto.ProgramPeriodId,
                        Status = item.Status,
                        FinalGrade = item.FinalGrade,
                        CompletionPercent = item.CompletionPercent
                    };
                    results.Add(record);
                    await unitOfWork.ProgressRecordRepository.AddAsync(record);
                }
            }

            await unitOfWork.CommitAsync();

            return AthenaApiResponse<List<ProgressRecordResponseDto>>.Ok(
                results.Select(r => new ProgressRecordResponseDto(r)).ToList());
        }
    }
}
