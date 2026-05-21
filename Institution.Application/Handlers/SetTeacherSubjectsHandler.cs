using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class SetTeacherSubjectsHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<SetTeacherSubjectsCommand, AthenaApiResponse<TeacherResponseDto>>
    {
        public async Task<AthenaApiResponse<TeacherResponseDto>> Handle(SetTeacherSubjectsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<TeacherResponseDto>.NotFound("Institution not found.");

            var teacher = await unitOfWork.TeacherRepository.FindByIdAsync(request.Dto.TeacherId);
            if (teacher == null || teacher.InstitutionId != institution.Id)
                return AthenaApiResponse<TeacherResponseDto>.NotFound("Teacher not found.");

            foreach (var subjectId in request.Dto.SubjectIds)
            {
                var subject = await unitOfWork.SubjectRepository.FindByIdAsync(subjectId);
                if (subject == null)
                    return AthenaApiResponse<TeacherResponseDto>.NotFound($"Subject {subjectId} not found.");
            }

            var newSubjects = request.Dto.SubjectIds.Select(subjectId => new TeacherSubjectEntity
            {
                TeacherId = teacher.Id,
                Teacher = teacher,
                SubjectId = subjectId,
                Subject = null!
            }).ToList();

            await unitOfWork.TeacherRepository.SetSubjectsAsync(teacher.Id, newSubjects);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<TeacherResponseDto>.Ok(new TeacherResponseDto(teacher));
        }
    }
}
