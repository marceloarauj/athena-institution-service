using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateTeacherHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CreateTeacherCommand, AthenaApiResponse<TeacherResponseDto>>
    {
        public async Task<AthenaApiResponse<TeacherResponseDto>> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<TeacherResponseDto>.NotFound("Institution not found.");

            var entity = new TeacherEntity
            {
                UserId = request.Dto.UserId,
                Name = request.Dto.Name,
                Email = request.Dto.Email,
                InstitutionId = institution.Id,
                Institution = institution
            };

            await unitOfWork.TeacherRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<TeacherResponseDto>.Created(new TeacherResponseDto(entity));
        }
    }
}
