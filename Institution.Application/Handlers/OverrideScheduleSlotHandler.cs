using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class OverrideScheduleSlotHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<OverrideScheduleSlotCommand, AthenaApiResponse<ClassScheduleResponseDto>>
    {
        public async Task<AthenaApiResponse<ClassScheduleResponseDto>> Handle(OverrideScheduleSlotCommand request, CancellationToken cancellationToken)
        {
            var schedules = await unitOfWork.ClassScheduleRepository.GetByGroupAsync(request.Dto.ClassGroupId);
            var schedule = schedules.FirstOrDefault(s => s.Id == request.ScheduleId);

            if (schedule == null)
                return AthenaApiResponse<ClassScheduleResponseDto>.NotFound("Schedule slot not found.");

            schedule.SubjectId = request.Dto.SubjectId;
            schedule.TeacherId = request.Dto.TeacherId;
            schedule.DayOfWeek = request.Dto.DayOfWeek;
            schedule.ScheduleSlotId = request.Dto.ScheduleSlotId;

            await unitOfWork.ClassScheduleRepository.UpdateAsync(schedule);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ClassScheduleResponseDto>.Ok(new ClassScheduleResponseDto(schedule));
        }
    }
}
