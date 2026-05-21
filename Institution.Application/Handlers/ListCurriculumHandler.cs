using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListCurriculumHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<ListCurriculumCommand, AthenaApiResponse<List<CurriculumEntryResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<CurriculumEntryResponseDto>>> Handle(ListCurriculumCommand request, CancellationToken cancellationToken)
        {
            var entries = await unitOfWork.CurriculumEntryRepository.GetByEditionAsync(request.ProgramEditionId, request.GradeOrYear);
            return AthenaApiResponse<List<CurriculumEntryResponseDto>>.Ok(entries.Select(e => new CurriculumEntryResponseDto(e)).ToList());
        }
    }
}
