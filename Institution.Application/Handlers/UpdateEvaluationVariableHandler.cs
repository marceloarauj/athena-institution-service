using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class UpdateEvaluationVariableHandler : IMessageHandler<UpdateEvaluationVariableCommand, AthenaApiResponse<UpdateEvaluationVariableResponseDto>>
    {
        public Task<AthenaApiResponse<UpdateEvaluationVariableResponseDto>> Handle(UpdateEvaluationVariableCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}