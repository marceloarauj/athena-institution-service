using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CheckInstitutionExistsHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<CheckInstitutionExistsCommand, AthenaApiResponse<bool>>
    {
        public async Task<AthenaApiResponse<bool>> Handle(CheckInstitutionExistsCommand request, CancellationToken cancellationToken)
        {
            var exists = await unitOfWork.InstitutionRepository.ExistsByAliasAsync(request.Alias);
            return AthenaApiResponse<bool>.Ok(exists);
        }
    }
}
