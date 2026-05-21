using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class SetTeacherAvailabilityHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<SetTeacherAvailabilityCommand, AthenaApiResponse<List<TeacherAvailabilityResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<TeacherAvailabilityResponseDto>>> Handle(SetTeacherAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<TeacherAvailabilityResponseDto>>.NotFound("Institution not found.");

            var teacher = await unitOfWork.TeacherRepository.FindByIdAsync(request.Dto.TeacherId);
            if (teacher == null || teacher.InstitutionId != institution.Id)
                return AthenaApiResponse<List<TeacherAvailabilityResponseDto>>.NotFound("Teacher not found.");

            foreach (var item in request.Dto.Availabilities)
            {
                var shift = await unitOfWork.ShiftRepository.FindByIdAsync(item.ShiftId);
                if (shift == null || shift.InstitutionId != institution.Id)
                    return AthenaApiResponse<List<TeacherAvailabilityResponseDto>>.NotFound($"Shift {item.ShiftId} not found.");
            }

            var availabilities = request.Dto.Availabilities.Select(item => new TeacherAvailabilityEntity
            {
                TeacherId = teacher.Id,
                Teacher = teacher,
                DayOfWeek = item.DayOfWeek,
                ShiftId = item.ShiftId,
                Shift = null!
            }).ToList();

            await unitOfWork.TeacherRepository.SetAvailabilityAsync(teacher.Id, availabilities);
            await unitOfWork.CommitAsync();

            var saved = await unitOfWork.TeacherRepository.GetAvailabilityAsync(teacher.Id);
            return AthenaApiResponse<List<TeacherAvailabilityResponseDto>>.Ok(saved.Select(a => new TeacherAvailabilityResponseDto(a)).ToList());
        }
    }
}
