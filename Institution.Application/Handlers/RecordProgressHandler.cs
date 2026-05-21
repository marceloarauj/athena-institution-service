using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class RecordProgressHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<RecordProgressCommand, AthenaApiResponse<ProgressRecordResponseDto>>
    {
        public async Task<AthenaApiResponse<ProgressRecordResponseDto>> Handle(RecordProgressCommand request, CancellationToken cancellationToken)
        {
            var existing = await unitOfWork.ProgressRecordRepository.FindByEnrollmentAndPeriodAsync(
                request.Dto.EnrollmentId, request.Dto.ProgramPeriodId);

            if (existing != null)
            {
                existing.Status = request.Dto.Status;
                existing.FinalGrade = request.Dto.FinalGrade;
                existing.CompletionPercent = request.Dto.CompletionPercent;
                await unitOfWork.ProgressRecordRepository.UpdateAsync(existing);
                await unitOfWork.CommitAsync();
                return AthenaApiResponse<ProgressRecordResponseDto>.Ok(new ProgressRecordResponseDto(existing));
            }

            var record = new ProgressRecordEntity
            {
                EnrollmentId = request.Dto.EnrollmentId,
                ProgramPeriodId = request.Dto.ProgramPeriodId,
                Status = request.Dto.Status,
                FinalGrade = request.Dto.FinalGrade,
                CompletionPercent = request.Dto.CompletionPercent
            };

            await unitOfWork.ProgressRecordRepository.AddAsync(record);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ProgressRecordResponseDto>.Created(new ProgressRecordResponseDto(record));
        }
    }
}
