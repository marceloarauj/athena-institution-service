using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GenerateClassGroupsHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<GenerateClassGroupsCommand, AthenaApiResponse<List<ClassGroupResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ClassGroupResponseDto>>> Handle(GenerateClassGroupsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<ClassGroupResponseDto>>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<List<ClassGroupResponseDto>>.NotFound("Program edition not found.");

            if (edition.Status == EditionStatus.Published || edition.Status == EditionStatus.Closed)
                return AthenaApiResponse<List<ClassGroupResponseDto>>.UnprocessableEntity("Cannot modify a published or closed edition.");

            var groups = new List<ClassGroupEntity>();

            foreach (var config in request.Dto.Groups)
            {
                for (int i = 0; i < config.NumberOfGroups; i++)
                {
                    var suffix = (char)('A' + i);
                    var name = config.GradeOrYear > 0
                        ? $"{config.GradeOrYear}{suffix}"
                        : $"Turma {suffix}";

                    groups.Add(new ClassGroupEntity
                    {
                        Name = name,
                        GradeOrYear = config.GradeOrYear,
                        RoomId = config.RoomId,
                        ShiftId = config.ShiftId,
                        MaxStudents = config.MaxStudents,
                        ProgramEditionId = edition.Id,
                        ProgramEdition = edition
                    });
                }
            }

            await unitOfWork.ClassGroupRepository.AddRangeAsync(groups);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<List<ClassGroupResponseDto>>.Created(
                groups.Select(g => new ClassGroupResponseDto(g)).ToList());
        }
    }
}
