using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListClassGroupsHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<ListClassGroupsCommand, AthenaApiResponse<List<ClassGroupResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ClassGroupResponseDto>>> Handle(ListClassGroupsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<ClassGroupResponseDto>>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<List<ClassGroupResponseDto>>.NotFound("Program edition not found.");

            var groups = await unitOfWork.ClassGroupRepository.GetByEditionAsync(edition.Id);
            return AthenaApiResponse<List<ClassGroupResponseDto>>.Ok(
                groups.Select(g => new ClassGroupResponseDto(g)).ToList());
        }
    }
}
