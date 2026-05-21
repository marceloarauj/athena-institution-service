using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetPublishChecklistHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<GetPublishChecklistCommand, AthenaApiResponse<PublishChecklistDto>>
    {
        public async Task<AthenaApiResponse<PublishChecklistDto>> Handle(GetPublishChecklistCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<PublishChecklistDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<PublishChecklistDto>.NotFound("Program edition not found.");

            var checklist = await PublishProgramEditionHandler.BuildChecklist(unitOfWork, edition);
            return AthenaApiResponse<PublishChecklistDto>.Ok(checklist);
        }
    }
}
