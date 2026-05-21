using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetAcademicProgramHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<GetAcademicProgramCommand, AthenaApiResponse<AcademicProgramResponseDto>>
    {
        public async Task<AthenaApiResponse<AcademicProgramResponseDto>> Handle(GetAcademicProgramCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<AcademicProgramResponseDto>.NotFound("Institution not found.");

            var program = await unitOfWork.AcademicProgramRepository.FindByIdAsync(request.Id);
            if (program == null || program.InstitutionId != institution.Id)
                return AthenaApiResponse<AcademicProgramResponseDto>.NotFound("Academic program not found.");

            return AthenaApiResponse<AcademicProgramResponseDto>.Ok(new AcademicProgramResponseDto(program));
        }
    }
}
