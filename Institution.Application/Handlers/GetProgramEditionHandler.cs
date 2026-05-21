using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetProgramEditionHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<GetProgramEditionCommand, AthenaApiResponse<ProgramEditionResponseDto>>
    {
        public async Task<AthenaApiResponse<ProgramEditionResponseDto>> Handle(GetProgramEditionCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ProgramEditionResponseDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Id);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<ProgramEditionResponseDto>.NotFound("Program edition not found.");

            return AthenaApiResponse<ProgramEditionResponseDto>.Ok(new ProgramEditionResponseDto(edition));
        }
    }
}
