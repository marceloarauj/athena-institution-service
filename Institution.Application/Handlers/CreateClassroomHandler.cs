using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Auth;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateClassroomHandler
    (
        IUnitOfWork unitOfWork,
        IInstitutionContext institutionContext,
        IUser user
    ) : IMessageHandler<CreateClassroomCommand, AthenaApiResponse<ClassroomResponseDto>>
    {
        public async Task<AthenaApiResponse<ClassroomResponseDto>> Handle(CreateClassroomCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);

            if (institution == null)
                return AthenaApiResponse<ClassroomResponseDto>.NotFound("Institution not found.");

            var discipline = await unitOfWork.DisciplineRepository.FindByIdAsync(request.Dto.DisciplineId);

            if (discipline == null || discipline.InstitutionId != institution.Id)
                return AthenaApiResponse<ClassroomResponseDto>.NotFound("Discipline not found.");

            var classroom = request.Dto.ToEntity(discipline, user.UserId);

            await unitOfWork.ClassroomRepository.AddAsync(classroom);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ClassroomResponseDto>.Created(new ClassroomResponseDto(classroom));
        }
    }
}
