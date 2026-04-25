using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class UpdateAttendanceHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<UpdateAttendanceCommand, AthenaApiResponse<List<AttendanceResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<AttendanceResponseDto>>> Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
        {
            var dayLesson = await unitOfWork.DayLessonRepository.FindByIdAsync(request.DayLessonId);

            if (dayLesson == null)
                return AthenaApiResponse<List<AttendanceResponseDto>>.NotFound("DayLesson not found.");

            if (dayLesson.CanceledAt.HasValue)
                return AthenaApiResponse<List<AttendanceResponseDto>>.BadRequest("DayLesson is canceled.");

            if (dayLesson.ClassroomId == null)
                return AthenaApiResponse<List<AttendanceResponseDto>>.BadRequest("DayLesson has no associated classroom.");

            var studentIds = request.Dto.Students.Select(s => s.StudentId).ToList();

            var activeRegistrations = await unitOfWork.StudentClassroomRegistrationRepository
                .GetActiveByStudentIdsAsync(studentIds, dayLesson.ClassroomId.Value);

            var activeStudentIds = activeRegistrations.Select(r => r.StudentId).ToHashSet();
            var missingStudentId = studentIds.FirstOrDefault(id => !activeStudentIds.Contains(id));

            if (missingStudentId != default)
                return AthenaApiResponse<List<AttendanceResponseDto>>.BadRequest(
                    $"Student {missingStudentId} is not registered or not active in this classroom.");

            var existingRecords = await unitOfWork.StudentDayLessonRepository
                .GetByDayLessonIdAsync(request.DayLessonId);

            var existingByStudent = existingRecords.ToDictionary(record => record.StudentId);

            var toAdd = new List<StudentDayLesson>();
            var processed = new List<StudentDayLesson>();

            foreach (var student in request.Dto.Students)
            {
                if (existingByStudent.TryGetValue(student.StudentId, out var existing))
                {
                    existing.IsPresent = student.IsPresent;
                    existing.Observations = student.Observation;
                    processed.Add(existing);
                }
                else
                {
                    var record = new StudentDayLesson
                    {
                        StudentId = student.StudentId,
                        DayLessonId = request.DayLessonId,
                        IsPresent = student.IsPresent,
                        Observations = student.Observation
                    };
                    toAdd.Add(record);
                    processed.Add(record);
                }
            }

            if (toAdd.Count > 0)
                await unitOfWork.StudentDayLessonRepository.AddRangeAsync(toAdd);

            await unitOfWork.CommitAsync();

            return AthenaApiResponse<List<AttendanceResponseDto>>.Ok(
                [.. processed.Select(attendance => new AttendanceResponseDto(attendance))]);
        }
    }
}
