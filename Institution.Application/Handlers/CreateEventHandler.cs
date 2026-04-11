using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateEventHandler
    (
        IUnitOfWork unitOfWork,
        IInstitutionContext institutionContext
    ) : IMessageHandler<CreateEventCommand, AthenaApiResponse<CreateEventResponseDto>>
    {
        public async Task<AthenaApiResponse<CreateEventResponseDto>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            if (dto.EndDate <= dto.StartDate)
                return AthenaApiResponse<CreateEventResponseDto>.UnprocessableEntity("End date must be after start date.");

            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);

            if (institution == null)
                return AthenaApiResponse<CreateEventResponseDto>.NotFound("Institution not found.");

            var @event = dto.ToEntity(institution);

            await unitOfWork.EventRepository.AddAsync(@event);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<CreateEventResponseDto>.Created(new CreateEventResponseDto(@event));
        }
    }
}
