using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListTeachersHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<ListTeachersCommand, AthenaApiResponse<List<TeacherResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<TeacherResponseDto>>> Handle(ListTeachersCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<TeacherResponseDto>>.NotFound("Institution not found.");

            var teachers = await unitOfWork.TeacherRepository.GetByInstitutionAsync(institution.Id);
            return AthenaApiResponse<List<TeacherResponseDto>>.Ok(teachers.Select(t => new TeacherResponseDto(t)).ToList());
        }
    }
}
