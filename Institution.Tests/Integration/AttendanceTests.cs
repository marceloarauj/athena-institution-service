using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Institution.Application.Handlers;
using Institution.Application.Interfaces;
using Institution.Application.Interfaces.Repositories;
using Institution.Controllers;
using Institution.Domain.Entities;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Institution.Tests.Integration
{
    public class AttendanceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IDayLessonRepository> _dayLessonRepoMock = new();
        private readonly Mock<IStudentClassroomRegistrationRepository> _registrationRepoMock = new();
        private readonly Mock<IStudentDayLessonRepository> _studentDayLessonRepoMock = new();
        private readonly DayLessonController _controller;

        public AttendanceTests()
        {
            _unitOfWorkMock.Setup(u => u.DayLessonRepository).Returns(_dayLessonRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.StudentClassroomRegistrationRepository).Returns(_registrationRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.StudentDayLessonRepository).Returns(_studentDayLessonRepoMock.Object);

            var services = new ServiceCollection();
            services.AddSingleton(_unitOfWorkMock.Object);
            services.AddScoped<IMessageHandler<UpdateAttendanceCommand, AthenaApiResponse<List<AttendanceResponseDto>>>, UpdateAttendanceHandler>();

            var provider = services.BuildServiceProvider();
            var mediator = new Mediator.Mediator.Mediator(provider);

            _controller = new DayLessonController(mediator);
        }

        private static DayLessonEntity BuildDayLesson(Guid classroomId, bool canceled = false) => new()
        {
            Id = Guid.NewGuid(),
            Location = "Room 101",
            StartDate = new DateTime(2026, 4, 25, 8, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2026, 4, 25, 10, 0, 0, DateTimeKind.Utc),
            TeacherId = Guid.NewGuid(),
            ClassroomId = classroomId,
            CanceledAt = canceled ? DateTime.UtcNow : null
        };

        private static StudentClassroomRegistrationEntity BuildRegistration(Guid studentId, Guid classroomId, bool active = true) => new()
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            StudentName = "Test Student",
            IsActive = active,
            ClassroomId = classroomId,
            Classroom = null!
        };

        [Fact]
        public async Task UpdateAttendance_ReturnsOk_WhenCreatingNewRecords()
        {
            var classroomId = Guid.NewGuid();
            var dayLesson = BuildDayLesson(classroomId);
            var studentId1 = Guid.NewGuid();
            var studentId2 = Guid.NewGuid();

            _dayLessonRepoMock
                .Setup(r => r.FindByIdAsync(dayLesson.Id))
                .ReturnsAsync(dayLesson);

            _registrationRepoMock
                .Setup(r => r.GetActiveByStudentIdsAsync(It.IsAny<List<Guid>>(), classroomId))
                .ReturnsAsync([
                    BuildRegistration(studentId1, classroomId),
                    BuildRegistration(studentId2, classroomId)
                ]);

            _studentDayLessonRepoMock
                .Setup(r => r.GetByDayLessonIdAsync(dayLesson.Id))
                .ReturnsAsync([]);

            var dto = new UpdateAttendanceDto
            {
                Students =
                [
                    new StudentAttendanceDto { StudentId = studentId1, IsPresent = true, Observation = "On time" },
                    new StudentAttendanceDto { StudentId = studentId2, IsPresent = false }
                ]
            };

            var result = await _controller.UpdateAttendance(dayLesson.Id, dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<List<AttendanceResponseDto>>>(objectResult.Value);
            Assert.True(body.Success);
            Assert.Equal(2, body.Data!.Count);

            _studentDayLessonRepoMock.Verify(r => r.AddRangeAsync(It.Is<List<StudentDayLesson>>(l => l.Count == 2)), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAttendance_ReturnsOk_WhenUpdatingExistingRecords()
        {
            var classroomId = Guid.NewGuid();
            var dayLesson = BuildDayLesson(classroomId);
            var studentId = Guid.NewGuid();

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

            _registrationRepoMock
                .Setup(r => r.GetActiveByStudentIdsAsync(It.IsAny<List<Guid>>(), classroomId))
                .ReturnsAsync([BuildRegistration(studentId, classroomId)]);

            _studentDayLessonRepoMock
                .Setup(r => r.GetByDayLessonIdAsync(dayLesson.Id))
                .ReturnsAsync([existingRecord]);

            var dto = new UpdateAttendanceDto
            {
                Students = [new StudentAttendanceDto { StudentId = studentId, IsPresent = true, Observation = "Late" }]
            };

            var result = await _controller.UpdateAttendance(dayLesson.Id, dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<List<AttendanceResponseDto>>>(objectResult.Value);
            Assert.True(body.Data![0].IsPresent);
            Assert.Equal("Late", body.Data[0].Observation);

            _studentDayLessonRepoMock.Verify(r => r.AddRangeAsync(It.IsAny<List<StudentDayLesson>>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAttendance_ReturnsNotFound_WhenDayLessonNotFound()
        {
            _dayLessonRepoMock
                .Setup(r => r.FindByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((DayLessonEntity?)null);

            var dto = new UpdateAttendanceDto
            {
                Students = [new StudentAttendanceDto { StudentId = Guid.NewGuid(), IsPresent = true }]
            };

            var result = await _controller.UpdateAttendance(Guid.NewGuid(), dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAttendance_ReturnsBadRequest_WhenDayLessonIsCanceled()
        {
            var classroomId = Guid.NewGuid();
            var dayLesson = BuildDayLesson(classroomId, canceled: true);

            _dayLessonRepoMock
                .Setup(r => r.FindByIdAsync(dayLesson.Id))
                .ReturnsAsync(dayLesson);

            var dto = new UpdateAttendanceDto
            {
                Students = [new StudentAttendanceDto { StudentId = Guid.NewGuid(), IsPresent = true }]
            };

            var result = await _controller.UpdateAttendance(dayLesson.Id, dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAttendance_ReturnsBadRequest_WhenStudentNotRegisteredInClassroom()
        {
            var classroomId = Guid.NewGuid();
            var dayLesson = BuildDayLesson(classroomId);
            var registeredStudentId = Guid.NewGuid();
            var unknownStudentId = Guid.NewGuid();

            _dayLessonRepoMock
                .Setup(r => r.FindByIdAsync(dayLesson.Id))
                .ReturnsAsync(dayLesson);

            _registrationRepoMock
                .Setup(r => r.GetActiveByStudentIdsAsync(It.IsAny<List<Guid>>(), classroomId))
                .ReturnsAsync([BuildRegistration(registeredStudentId, classroomId)]);

            var dto = new UpdateAttendanceDto
            {
                Students =
                [
                    new StudentAttendanceDto { StudentId = registeredStudentId, IsPresent = true },
                    new StudentAttendanceDto { StudentId = unknownStudentId, IsPresent = false }
                ]
            };

            var result = await _controller.UpdateAttendance(dayLesson.Id, dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAttendance_ReturnsBadRequest_WhenStudentIsInactive()
        {
            var classroomId = Guid.NewGuid();
            var dayLesson = BuildDayLesson(classroomId);
            var inactiveStudentId = Guid.NewGuid();

            _dayLessonRepoMock
                .Setup(r => r.FindByIdAsync(dayLesson.Id))
                .ReturnsAsync(dayLesson);

            _registrationRepoMock
                .Setup(r => r.GetActiveByStudentIdsAsync(It.IsAny<List<Guid>>(), classroomId))
                .ReturnsAsync([]);

            var dto = new UpdateAttendanceDto
            {
                Students = [new StudentAttendanceDto { StudentId = inactiveStudentId, IsPresent = true }]
            };

            var result = await _controller.UpdateAttendance(dayLesson.Id, dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
