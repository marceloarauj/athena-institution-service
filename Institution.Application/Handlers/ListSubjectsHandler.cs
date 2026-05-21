using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListSubjectsHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<ListSubjectsCommand, AthenaApiResponse<List<SubjectResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<SubjectResponseDto>>> Handle(ListSubjectsCommand request, CancellationToken cancellationToken)
        {
            var subjects = await unitOfWork.SubjectRepository.GetByProgramAsync(request.AcademicProgramId);
            return AthenaApiResponse<List<SubjectResponseDto>>.Ok(subjects.Select(s => new SubjectResponseDto(s)).ToList());
        }
    }
}
