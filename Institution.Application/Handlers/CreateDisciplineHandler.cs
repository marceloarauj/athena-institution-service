using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateDisciplineHandler
    (
        IUnitOfWork unitOfWork,
        IInstitutionContext institutionContext
    ) : IMessageHandler<CreateDisciplineCommand, AthenaApiResponse<CreateDisciplineResponseDto>>
    {
        public async Task<AthenaApiResponse<CreateDisciplineResponseDto>> Handle(CreateDisciplineCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);

            if (institution == null)
                return AthenaApiResponse<CreateDisciplineResponseDto>.NotFound("Institution not found.");

            var discipline = request.Dto.ToEntity(institution);

            await unitOfWork.DisciplineRepository.AddAsync(discipline);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<CreateDisciplineResponseDto>.Created(new CreateDisciplineResponseDto(discipline));
        }
    }
}
