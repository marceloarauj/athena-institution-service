using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetClassGroupHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<GetClassGroupCommand, AthenaApiResponse<ClassGroupResponseDto>>
    {
        public async Task<AthenaApiResponse<ClassGroupResponseDto>> Handle(GetClassGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await unitOfWork.ClassGroupRepository.FindByIdAsync(request.Id);
            if (group == null)
                return AthenaApiResponse<ClassGroupResponseDto>.NotFound("Class group not found.");

            return AthenaApiResponse<ClassGroupResponseDto>.Ok(new ClassGroupResponseDto(group));
        }
    }
}
