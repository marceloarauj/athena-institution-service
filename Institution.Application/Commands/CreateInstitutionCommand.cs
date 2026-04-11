using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record CreateInstitutionCommand(CreateInstitutionDto Dto) : IRequestMessage<AthenaApiResponse<CreateInstitutionResponseDto>>;
}
