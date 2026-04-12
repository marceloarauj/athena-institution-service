using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListClassroomsHandler
    (
        IUnitOfWork unitOfWork,
        IInstitutionContext institutionContext
    ) : IMessageHandler<ListClassroomsCommand, AthenaApiResponse<List<ClassroomResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ClassroomResponseDto>>> Handle(ListClassroomsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);

            if (institution == null)
                return AthenaApiResponse<List<ClassroomResponseDto>>.NotFound("Institution not found.");

            var filter = request.Filter;

            var classrooms = await unitOfWork.ClassroomRepository.GetByFilterAsync(
                institution.Id,
                filter.DisciplineId,
                filter.TeacherId,
                filter.StartDate,
                filter.EndDate
            );

            var response = classrooms.Select(classroom => new ClassroomResponseDto(classroom)).ToList();

            return AthenaApiResponse<List<ClassroomResponseDto>>.Ok(response);
        }
    }
}
