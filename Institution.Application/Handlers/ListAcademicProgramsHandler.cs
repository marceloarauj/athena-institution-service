using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListAcademicProgramsHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<ListAcademicProgramsCommand, AthenaApiResponse<List<AcademicProgramResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<AcademicProgramResponseDto>>> Handle(ListAcademicProgramsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<AcademicProgramResponseDto>>.NotFound("Institution not found.");

            var programs = await unitOfWork.AcademicProgramRepository.GetByInstitutionAsync(institution.Id);
            return AthenaApiResponse<List<AcademicProgramResponseDto>>.Ok(programs.Select(p => new AcademicProgramResponseDto(p)).ToList());
        }
    }
}
