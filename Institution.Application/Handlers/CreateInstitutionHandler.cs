using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Auth;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Application.Interfaces.Services;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateInstitutionHandler
    (
        IUnitOfWork unitOfWork, 
        IInstitutionService institutionService,
        IUser user
    ) : IMessageHandler<CreateInstitutionCommand, AthenaApiResponse<CreateInstitutionResponseDto>>
    {
        public async Task<AthenaApiResponse<CreateInstitutionResponseDto>> Handle(CreateInstitutionCommand request, CancellationToken cancellationToken)
        {
            bool aliasExists = await unitOfWork.InstitutionRepository.ExistsByAliasAsync(request.Dto.Alias);
            
            if(aliasExists)
                return AthenaApiResponse<CreateInstitutionResponseDto>.UnprocessableEntity("Institution alias already exists.");

            var institution = request.Dto.ToEntity();

            if (request.Dto.Logo != null)
                await institutionService.UpdateInstitutionLogo(request.Dto.Logo);

            institution.CreatedBy = user.UserId;

            await unitOfWork.InstitutionRepository.AddAsync(institution);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<CreateInstitutionResponseDto>.Created(new CreateInstitutionResponseDto(institution));
        }
    }
}
