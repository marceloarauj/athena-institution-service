using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class PatchStudentAttendanceHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<PatchStudentAttendanceCommand, AthenaApiResponse<AttendanceResponseDto>>
    {
        public async Task<AthenaApiResponse<AttendanceResponseDto>> Handle(PatchStudentAttendanceCommand request, CancellationToken cancellationToken)
        {
            var dayLesson = await unitOfWork.DayLessonRepository.FindByIdAsync(request.DayLessonId);

            if (dayLesson == null)
                return AthenaApiResponse<AttendanceResponseDto>.NotFound("DayLesson not found.");

            if (dayLesson.CanceledAt.HasValue)
                return AthenaApiResponse<AttendanceResponseDto>.BadRequest("DayLesson is canceled.");

            var record = await unitOfWork.StudentDayLessonRepository
                .FindByDayLessonAndStudentAsync(request.DayLessonId, request.StudentId);

            if (record == null)
                return AthenaApiResponse<AttendanceResponseDto>.NotFound("Attendance record not found for this student.");

            if (request.Dto.IsPresent.HasValue)
                record.IsPresent = request.Dto.IsPresent;

            if (request.Dto.Observation != null)
                record.Observations = request.Dto.Observation;

            await unitOfWork.CommitAsync();

            return AthenaApiResponse<AttendanceResponseDto>.Ok(new AttendanceResponseDto(record));
        }
    }
}
