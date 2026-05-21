using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateSubjectHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CreateSubjectCommand, AthenaApiResponse<SubjectResponseDto>>
    {
        public async Task<AthenaApiResponse<SubjectResponseDto>> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<SubjectResponseDto>.NotFound("Institution not found.");

            var program = await unitOfWork.AcademicProgramRepository.FindByIdAsync(request.Dto.AcademicProgramId);
            if (program == null || program.InstitutionId != institution.Id)
                return AthenaApiResponse<SubjectResponseDto>.NotFound("Academic program not found.");

            var entity = new SubjectEntity
            {
                Name = request.Dto.Name,
                Code = request.Dto.Code,
                AcademicProgramId = program.Id,
                AcademicProgram = program
            };

            await unitOfWork.SubjectRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<SubjectResponseDto>.Created(new SubjectResponseDto(entity));
        }
    }
}
