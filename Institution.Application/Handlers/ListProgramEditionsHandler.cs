using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListProgramEditionsHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<ListProgramEditionsCommand, AthenaApiResponse<List<ProgramEditionResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ProgramEditionResponseDto>>> Handle(ListProgramEditionsCommand request, CancellationToken cancellationToken)
        {
            var editions = await unitOfWork.ProgramEditionRepository.GetByProgramAsync(request.AcademicProgramId);
            return AthenaApiResponse<List<ProgramEditionResponseDto>>.Ok(editions.Select(e => new ProgramEditionResponseDto(e)).ToList());
        }
    }
}
