using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Auth;
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
    public class ClassroomTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IInstitutionRepository> _institutionRepoMock = new();
        private readonly Mock<IDisciplineRepository> _disciplineRepoMock = new();
        private readonly Mock<IClassroomRepository> _classroomRepoMock = new();
        private readonly Mock<IInstitutionContext> _institutionContextMock = new();
        private readonly Mock<IUser> _userMock = new();
        private readonly Guid _userId = Guid.NewGuid();
        private readonly ClassroomController _controller;

        public ClassroomTests()
        {
            _unitOfWorkMock.Setup(u => u.InstitutionRepository).Returns(_institutionRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.DisciplineRepository).Returns(_disciplineRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.ClassroomRepository).Returns(_classroomRepoMock.Object);
            _institutionContextMock.Setup(c => c.Alias).Returns("test-institution");
            _userMock.Setup(u => u.UserId).Returns(_userId);

            var services = new ServiceCollection();
            services.AddSingleton(_unitOfWorkMock.Object);
            services.AddSingleton(_institutionContextMock.Object);
            services.AddSingleton(_userMock.Object);
            services.AddScoped<IMessageHandler<CreateClassroomCommand, AthenaApiResponse<ClassroomResponseDto>>, CreateClassroomHandler>();
            services.AddScoped<IMessageHandler<ListClassroomsCommand, AthenaApiResponse<List<ClassroomResponseDto>>>, ListClassroomsHandler>();

            var provider = services.BuildServiceProvider();
            var mediator = new Mediator.Mediator.Mediator(provider);

            _controller = new ClassroomController(mediator);
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

        // --- CreateClassroom ---

        [Fact]
        public async Task CreateClassroom_ReturnsCreated_WhenSuccessful()
        {
            var institution = BuildInstitution();
            var discipline = BuildDiscipline(institution);

            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync("test-institution"))
                .ReturnsAsync(institution);

            _disciplineRepoMock
                .Setup(r => r.FindByIdAsync(discipline.Id))
                .ReturnsAsync(discipline);

            var dto = new CreateClassroomDto
            {
                DisciplineId = discipline.Id,
                TeacherId = Guid.NewGuid(),
                Location = "Room 101",
                StartDate = new DateTime(2026, 3, 1, 8, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 6, 30, 18, 0, 0, DateTimeKind.Utc),
                MaxStudents = 30
            };

            var result = await _controller.CreateClassroom(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<ClassroomResponseDto>>(objectResult.Value);
            Assert.True(body.Success);
            Assert.NotNull(body.Data);
            Assert.Equal(discipline.Id, body.Data.DisciplineId);
            Assert.Equal("Room 101", body.Data.Location);
            Assert.Equal(30, body.Data.MaxStudents);

            _classroomRepoMock.Verify(r => r.AddAsync(It.IsAny<ClassroomEntity>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateClassroom_ReturnsNotFound_WhenInstitutionNotFound()
        {
            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync(It.IsAny<string>()))
                .ReturnsAsync((InstitutionEntity?)null);

            var dto = new CreateClassroomDto
            {
                DisciplineId = Guid.NewGuid(),
                TeacherId = Guid.NewGuid(),
                Location = "Room 101",
                StartDate = new DateTime(2026, 3, 1, 8, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 6, 30, 18, 0, 0, DateTimeKind.Utc)
            };

            var result = await _controller.CreateClassroom(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
            _classroomRepoMock.Verify(r => r.AddAsync(It.IsAny<ClassroomEntity>()), Times.Never);
        }

        [Fact]
        public async Task CreateClassroom_ReturnsNotFound_WhenDisciplineNotFound()
        {
            var institution = BuildInstitution();

            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync("test-institution"))
                .ReturnsAsync(institution);

            _disciplineRepoMock
                .Setup(r => r.FindByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((DisciplineEntity?)null);

            var dto = new CreateClassroomDto
            {
                DisciplineId = Guid.NewGuid(),
                TeacherId = Guid.NewGuid(),
                Location = "Room 101",
                StartDate = new DateTime(2026, 3, 1, 8, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 6, 30, 18, 0, 0, DateTimeKind.Utc)
            };

            var result = await _controller.CreateClassroom(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
            _classroomRepoMock.Verify(r => r.AddAsync(It.IsAny<ClassroomEntity>()), Times.Never);
        }

        [Fact]
        public async Task CreateClassroom_ReturnsNotFound_WhenDisciplineBelongsToDifferentInstitution()
        {
            var institution = BuildInstitution();
            var otherInstitution = BuildInstitution();
            var discipline = BuildDiscipline(otherInstitution);

            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync("test-institution"))
                .ReturnsAsync(institution);

            _disciplineRepoMock
                .Setup(r => r.FindByIdAsync(discipline.Id))
                .ReturnsAsync(discipline);

            var dto = new CreateClassroomDto
            {
                DisciplineId = discipline.Id,
                TeacherId = Guid.NewGuid(),
                Location = "Room 101",
                StartDate = new DateTime(2026, 3, 1, 8, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 6, 30, 18, 0, 0, DateTimeKind.Utc)
            };

            var result = await _controller.CreateClassroom(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
            _classroomRepoMock.Verify(r => r.AddAsync(It.IsAny<ClassroomEntity>()), Times.Never);
        }

        // --- ListClassrooms ---

        [Fact]
        public async Task ListClassrooms_ReturnsOk_WithClassroomList()
        {
            var institution = BuildInstitution();
            var discipline = BuildDiscipline(institution);
            var classrooms = new List<ClassroomEntity>
            {
                BuildClassroom(discipline),
                BuildClassroom(discipline)
            };

            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync("test-institution"))
                .ReturnsAsync(institution);

            _classroomRepoMock
                .Setup(r => r.GetByFilterAsync(institution.Id, null, null, null, null))
                .ReturnsAsync(classrooms);

            var result = await _controller.ListClassrooms(new ListClassroomsFilterDto());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<List<ClassroomResponseDto>>>(objectResult.Value);
            Assert.True(body.Success);
            Assert.NotNull(body.Data);
            Assert.Equal(2, body.Data.Count);
        }

        [Fact]
        public async Task ListClassrooms_ReturnsOk_WithEmptyList()
        {
            var institution = BuildInstitution();

            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync("test-institution"))
                .ReturnsAsync(institution);

            _classroomRepoMock
                .Setup(r => r.GetByFilterAsync(institution.Id, null, null, null, null))
                .ReturnsAsync([]);

            var result = await _controller.ListClassrooms(new ListClassroomsFilterDto());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<List<ClassroomResponseDto>>>(objectResult.Value);
            Assert.True(body.Success);
            Assert.NotNull(body.Data);
            Assert.Empty(body.Data);
        }

        [Fact]
        public async Task ListClassrooms_ReturnsNotFound_WhenInstitutionNotFound()
        {
            _institutionRepoMock
                .Setup(r => r.FindByAliasAsync(It.IsAny<string>()))
                .ReturnsAsync((InstitutionEntity?)null);

            var result = await _controller.ListClassrooms(new ListClassroomsFilterDto());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }
    }
}
