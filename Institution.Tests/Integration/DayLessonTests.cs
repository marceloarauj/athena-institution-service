using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Institution.Application.Handlers;
using Institution.Application.Interfaces;
using Institution.Application.Interfaces.Repositories;
using Institution.Controllers;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Institution.Tests.Integration
{
    public class DayLessonTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IInstitutionContext> _institutionContextMock = new();
        private readonly Mock<IInstitutionRepository> _institutionRepoMock = new();
        private readonly Mock<IClassroomRepository> _classroomRepoMock = new();
        private readonly Mock<IDayLessonRepository> _dayLessonRepoMock = new();
        private readonly Mock<IStudentDayLessonRepository> _studentDayLessonRepoMock = new();
        private readonly DayLessonController _controller;

        public DayLessonTests()
        {
            _institutionContextMock.Setup(c => c.Alias).Returns("test-institution");
            _unitOfWorkMock.Setup(u => u.InstitutionRepository).Returns(_institutionRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.ClassroomRepository).Returns(_classroomRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.DayLessonRepository).Returns(_dayLessonRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.StudentDayLessonRepository).Returns(_studentDayLessonRepoMock.Object);

            var services = new ServiceCollection();
            services.AddSingleton(_unitOfWorkMock.Object);
            services.AddSingleton(_institutionContextMock.Object);
            services.AddScoped<IMessageHandler<ListDayLessonsCommand, AthenaApiResponse<List<DayLessonResponseDto>>>, ListDayLessonsHandler>();
            services.AddScoped<IMessageHandler<PatchStudentAttendanceCommand, AthenaApiResponse<AttendanceResponseDto>>, PatchStudentAttendanceHandler>();

            var provider = services.BuildServiceProvider();
            var mediator = new Mediator.Mediator.Mediator(provider);

            _controller = new DayLessonController(mediator);
        }

        private static InstitutionEntity BuildInstitution() => new()
        {
            Id = Guid.NewGuid(),
            Alias = "test-institution",
            DisplayName = "Test Institution",
            ChargePayment = false,
            PaymentFormat = PaymentFormat.Free
        };

        private static DisciplineEntity BuildDiscipline(InstitutionEntity institution) => new()
        {
            Id = Guid.NewGuid(),
            Name = "Mathematics",
            StudyHours = 60,
            Credits = 4,
            ChargePayment = false,
            Available = true,
            CreatedBy = Guid.NewGuid(),
            Institution = institution,
            InstitutionId = institution.Id
        };

        private static ClassroomEntity BuildClassroom(DisciplineEntity discipline) => new()
        {
            Id = Guid.NewGuid(),
            Location = "Room 101",
            TeacherId = Guid.NewGuid(),
            StartDate = new DateTime(2026, 3, 1, 8, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2026, 6, 30, 18, 0, 0, DateTimeKind.Utc),
            CreateByUserId = Guid.NewGuid(),
            DisciplineId = discipline.Id,
            Discipline = discipline
        };

        private static DayLessonEntity BuildDayLesson(Guid classroomId, bool canceled = false) => new()
        {
            Id = Guid.NewGuid(),
            Location = "Room 101",
            StartDate = new DateTime(2026, 4, 25, 8, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2026, 4, 25, 10, 0, 0, DateTimeKind.Utc),
            TeacherId = Guid.NewGuid(),
            ClassroomId = classroomId,
            CanceledAt = canceled ? DateTime.UtcNow : null,
            DayLessonDisciplineTopics = [],
            StudentDayLessons = []
        };

        // --- ListDayLessons ---

        [Fact]
        public async Task ListDayLessons_ReturnsOk_WithLessonsAndStudents()
        {
            var institution = BuildInstitution();
            var discipline = BuildDiscipline(institution);
            var classroom = BuildClassroom(discipline);

            var topic = new DisciplineTopicEntity
            {
                Id = Guid.NewGuid(),
                Content = "Algebra basics",
                LessonNumber = 1,
                DisciplineId = discipline.Id
            };

            var dayLesson = BuildDayLesson(classroom.Id);
            dayLesson.DayLessonDisciplineTopics =
            [
                new DayLessonDisciplineTopic
                {
                    Id = Guid.NewGuid(),
                    DayLessonId = dayLesson.Id,
                    DisciplineTopicId = topic.Id,
                    DisciplineTopic = topic
                }
            ];
            dayLesson.StudentDayLessons =
            [
                new StudentDayLesson
                {
                    Id = Guid.NewGuid(),
                    StudentId = Guid.NewGuid(),
                    DayLessonId = dayLesson.Id,
                    IsPresent = true,
                    Observations = "On time"
                }
            ];

            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync("test-institution"))
                .ReturnsAsync(institution);

            _classroomRepoMock
                .Setup(r => r.FindByIdAsync(classroom.Id))
                .ReturnsAsync(classroom);

            _dayLessonRepoMock
                .Setup(r => r.GetByClassroomIdAsync(classroom.Id))
                .ReturnsAsync([dayLesson]);

            var result = await _controller.ListDayLessons(classroom.Id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<List<DayLessonResponseDto>>>(objectResult.Value);
            
            Assert.True(body.Success);
            Assert.NotNull(body.Data);
            Assert.Single(body.Data!);
            Assert.Single(body.Data[0].Topics);
            Assert.Equal("Algebra basics", body.Data[0].Topics[0]);
            Assert.Single(body.Data[0].Students);
            Assert.True(body.Data[0].Students[0].IsPresent);
        }

        [Fact]
        public async Task ListDayLessons_ReturnsOk_WithEmptyList()
        {
            var institution = BuildInstitution();
            var discipline = BuildDiscipline(institution);
            var classroom = BuildClassroom(discipline);

            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync("test-institution"))
                .ReturnsAsync(institution);

            _classroomRepoMock
                .Setup(r => r.FindByIdAsync(classroom.Id))
                .ReturnsAsync(classroom);

            _dayLessonRepoMock
                .Setup(r => r.GetByClassroomIdAsync(classroom.Id))
                .ReturnsAsync([]);

            var result = await _controller.ListDayLessons(classroom.Id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<List<DayLessonResponseDto>>>(objectResult.Value);
            Assert.Empty(body.Data!);
        }

        [Fact]
        public async Task ListDayLessons_ReturnsNotFound_WhenInstitutionNotFound()
        {
            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync(It.IsAny<string>()))
                .ReturnsAsync((InstitutionEntity?)null);

            var result = await _controller.ListDayLessons(Guid.NewGuid());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async Task ListDayLessons_ReturnsNotFound_WhenClassroomBelongsToDifferentInstitution()
        {
            var institution = BuildInstitution();
            var otherInstitution = BuildInstitution();
            var discipline = BuildDiscipline(otherInstitution);
            var classroom = BuildClassroom(discipline);

            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync("test-institution"))
                .ReturnsAsync(institution);

            _classroomRepoMock
                .Setup(r => r.FindByIdAsync(classroom.Id))
                .ReturnsAsync(classroom);

            var result = await _controller.ListDayLessons(classroom.Id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
            _dayLessonRepoMock.Verify(r => r.GetByClassroomIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        // --- PatchStudentAttendance ---

        [Fact]
        public async Task PatchStudentAttendance_ReturnsOk_WhenPatchingBothFields()
        {
            var classroomId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var dayLesson = BuildDayLesson(classroomId);

            var existingRecord = new StudentDayLesson
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                DayLessonId = dayLesson.Id,
                IsPresent = false,
                Observations = null
            };

            _dayLessonRepoMock
                .Setup(r => r.FindByIdAsync(dayLesson.Id))
                .ReturnsAsync(dayLesson);

            _studentDayLessonRepoMock
                .Setup(r => r.FindByDayLessonAndStudentAsync(dayLesson.Id, studentId))
                .ReturnsAsync(existingRecord);

            var dto = new PatchAttendanceDto { IsPresent = true, Observation = "Participated well" };

            var result = await _controller.PatchStudentAttendance(dayLesson.Id, studentId, dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<AttendanceResponseDto>>(objectResult.Value);
            Assert.True(body.Data!.IsPresent);
            Assert.Equal("Participated well", body.Data.Observation);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task PatchStudentAttendance_ReturnsOk_WhenPatchingOnlyObservation()
        {
            var classroomId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var dayLesson = BuildDayLesson(classroomId);

            var existingRecord = new StudentDayLesson
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                DayLessonId = dayLesson.Id,
                IsPresent = true,
                Observations = "Old note"
            };

            _dayLessonRepoMock
                .Setup(r => r.FindByIdAsync(dayLesson.Id))
                .ReturnsAsync(dayLesson);

            _studentDayLessonRepoMock
                .Setup(r => r.FindByDayLessonAndStudentAsync(dayLesson.Id, studentId))
                .ReturnsAsync(existingRecord);

            var dto = new PatchAttendanceDto { Observation = "Updated note" };

            var result = await _controller.PatchStudentAttendance(dayLesson.Id, studentId, dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<AttendanceResponseDto>>(objectResult.Value);
            Assert.True(body.Data!.IsPresent);
            Assert.Equal("Updated note", body.Data.Observation);
        }

        [Fact]
        public async Task PatchStudentAttendance_ReturnsNotFound_WhenDayLessonNotFound()
        {
            _dayLessonRepoMock
                .Setup(r => r.FindByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((DayLessonEntity?)null);

            var result = await _controller.PatchStudentAttendance(Guid.NewGuid(), Guid.NewGuid(), new PatchAttendanceDto());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task PatchStudentAttendance_ReturnsBadRequest_WhenDayLessonIsCanceled()
        {
            var dayLesson = BuildDayLesson(Guid.NewGuid(), canceled: true);

            _dayLessonRepoMock
                .Setup(r => r.FindByIdAsync(dayLesson.Id))
                .ReturnsAsync(dayLesson);

            var result = await _controller.PatchStudentAttendance(dayLesson.Id, Guid.NewGuid(), new PatchAttendanceDto());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task PatchStudentAttendance_ReturnsNotFound_WhenAttendanceRecordNotFound()
        {
            var dayLesson = BuildDayLesson(Guid.NewGuid());

            _dayLessonRepoMock
                .Setup(r => r.FindByIdAsync(dayLesson.Id))
                .ReturnsAsync(dayLesson);

            _studentDayLessonRepoMock
                .Setup(r => r.FindByDayLessonAndStudentAsync(dayLesson.Id, It.IsAny<Guid>()))
                .ReturnsAsync((StudentDayLesson?)null);

            var result = await _controller.PatchStudentAttendance(dayLesson.Id, Guid.NewGuid(), new PatchAttendanceDto { IsPresent = true });

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
