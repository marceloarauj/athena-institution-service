using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListDayLessonsHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<ListDayLessonsCommand, AthenaApiResponse<List<DayLessonResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<DayLessonResponseDto>>> Handle(ListDayLessonsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);

            if (institution == null)
                return AthenaApiResponse<List<DayLessonResponseDto>>.NotFound("Institution not found.");

            var classroom = await unitOfWork.ClassroomRepository.FindByIdAsync(request.ClassroomId);

            if (classroom == null || classroom.Discipline.InstitutionId != institution.Id)
                return AthenaApiResponse<List<DayLessonResponseDto>>.NotFound("Classroom not found.");

            var dayLessons = await unitOfWork.DayLessonRepository.GetByClassroomIdAsync(request.ClassroomId);

            var response = dayLessons.Select(d => new DayLessonResponseDto(d)).ToList();

            return AthenaApiResponse<List<DayLessonResponseDto>>.Ok(response);
        }
    }
}
