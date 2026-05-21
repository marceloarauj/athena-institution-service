using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateClassGroupHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CreateClassGroupCommand, AthenaApiResponse<ClassGroupResponseDto>>
    {
        public async Task<AthenaApiResponse<ClassGroupResponseDto>> Handle(CreateClassGroupCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ClassGroupResponseDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<ClassGroupResponseDto>.NotFound("Program edition not found.");

            if (edition.Status == EditionStatus.Published || edition.Status == EditionStatus.Closed)
                return AthenaApiResponse<ClassGroupResponseDto>.UnprocessableEntity("Cannot modify a published or closed edition.");

            var group = new ClassGroupEntity
            {
                Name = request.Dto.Name,
                GradeOrYear = request.Dto.GradeOrYear,
                RoomId = request.Dto.RoomId,
                ShiftId = request.Dto.ShiftId,
                MaxStudents = request.Dto.MaxStudents,
                ProgramEditionId = edition.Id,
                ProgramEdition = edition
            };

            await unitOfWork.ClassGroupRepository.AddAsync(group);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ClassGroupResponseDto>.Created(new ClassGroupResponseDto(group));
        }
    }
}
