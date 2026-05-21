using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CloseProgramEditionHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CloseProgramEditionCommand, AthenaApiResponse<ProgramEditionResponseDto>>
    {
        public async Task<AthenaApiResponse<ProgramEditionResponseDto>> Handle(CloseProgramEditionCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ProgramEditionResponseDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<ProgramEditionResponseDto>.NotFound("Program edition not found.");

            if (edition.Status == EditionStatus.Closed)
                return AthenaApiResponse<ProgramEditionResponseDto>.UnprocessableEntity("Edition is already closed.");

            if (edition.Status == EditionStatus.Draft)
                return AthenaApiResponse<ProgramEditionResponseDto>.UnprocessableEntity("Cannot close a draft edition. Publish it first.");

            edition.Status = EditionStatus.Closed;

            await unitOfWork.ProgramEditionRepository.UpdateStatusAsync(edition);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ProgramEditionResponseDto>.Ok(new ProgramEditionResponseDto(edition));
        }
    }
}
