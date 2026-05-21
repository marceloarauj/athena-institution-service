using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetTeacherScheduleHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<GetTeacherScheduleCommand, AthenaApiResponse<List<ClassScheduleResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ClassScheduleResponseDto>>> Handle(GetTeacherScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedules = await unitOfWork.ClassScheduleRepository.GetByTeacherAsync(request.TeacherId, request.ProgramEditionId);
            return AthenaApiResponse<List<ClassScheduleResponseDto>>.Ok(
                schedules.Select(s => new ClassScheduleResponseDto(s)).ToList());
        }
    }
}
