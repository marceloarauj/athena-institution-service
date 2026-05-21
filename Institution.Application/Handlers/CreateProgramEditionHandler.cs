using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateProgramEditionHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CreateProgramEditionCommand, AthenaApiResponse<ProgramEditionResponseDto>>
    {
        public async Task<AthenaApiResponse<ProgramEditionResponseDto>> Handle(CreateProgramEditionCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ProgramEditionResponseDto>.NotFound("Institution not found.");

            if (request.Dto.StartDate >= request.Dto.EndDate)
                return AthenaApiResponse<ProgramEditionResponseDto>.BadRequest("StartDate must be before EndDate.");

            var program = await unitOfWork.AcademicProgramRepository.FindByIdAsync(request.Dto.AcademicProgramId);
            if (program == null || program.InstitutionId != institution.Id)
                return AthenaApiResponse<ProgramEditionResponseDto>.NotFound("Academic program not found.");

            var entity = new ProgramEditionEntity
            {
                Name = request.Dto.Name,
                StartDate = request.Dto.StartDate,
                EndDate = request.Dto.EndDate,
                AcademicProgramId = program.Id,
                AcademicProgram = program
            };

            await unitOfWork.ProgramEditionRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ProgramEditionResponseDto>.Created(new ProgramEditionResponseDto(entity));
        }
    }
}
