using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class DeleteDisciplineHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<DeleteDisciplineCommand, AthenaApiResponse<object>>
    {
        public async Task<AthenaApiResponse<object>> Handle(DeleteDisciplineCommand request, CancellationToken cancellationToken)
        {
            var discipline = await unitOfWork.DisciplineRepository.FindByIdAsync(request.Id);

            if (discipline == null)
                return AthenaApiResponse<object>.NotFound("Discipline not found.");

            await unitOfWork.DisciplineRepository.DeleteAsync(discipline);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<object>.Ok(null!);
        }
    }
}
