using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Institution.Controllers;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Institution.Tests.Integration
{
    public class DisciplineControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly DisciplineController _controller;

        public DisciplineControllerTests()
        {
            _controller = new DisciplineController(_mediatorMock.Object);
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

        private static List<DisciplineTopicEntity> BuildTopics(DisciplineEntity discipline) =>
        [
            new() { Id = Guid.NewGuid(), Content = "Introduction to Algebra", LessonNumber = 1, DisciplineId = discipline.Id, Discipline = discipline },
            new() { Id = Guid.NewGuid(), Content = "Calculus Fundamentals", LessonNumber = 2, DisciplineId = discipline.Id, Discipline = discipline }
        ];

        [Fact]
        public async Task CreateDiscipline_ReturnsCreated_WhenSuccessful()
        {
            var institution = BuildInstitution();
            var discipline = BuildDiscipline(institution);
            var topics = BuildTopics(discipline);
            discipline.Topics = topics;

            var response = AthenaApiResponse<CreateDisciplineResponseDto>.Created(new CreateDisciplineResponseDto(discipline));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateDisciplineCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var dto = new CreateDisciplineDto
            {
                Name = discipline.Name,
                StudyHours = discipline.StudyHours,
                Credits = discipline.Credits,
                ChargePayment = discipline.ChargePayment,
                Topics =
                [
                    new CreateDisciplineTopicDto { Content = "Introduction to Algebra", LessonNumber = 1 },
                    new CreateDisciplineTopicDto { Content = "Calculus Fundamentals", LessonNumber = 2 }
                ]
            };

            var result = await _controller.CreateDiscipline(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<CreateDisciplineResponseDto>>(objectResult.Value);
            
            Assert.True(body.Success);
            Assert.NotNull(body.Data);
            Assert.Equal(discipline.Id, body.Data.Id);
            Assert.Equal(discipline.Name, body.Data.Name);
            Assert.Equal(discipline.StudyHours, body.Data.StudyHours);
            Assert.Equal(discipline.Credits, body.Data.Credits);
            Assert.Equal(discipline.ChargePayment, body.Data.ChargePayment);
            Assert.Equal(discipline.Available, body.Data.Available);
            Assert.Equal(institution.Id, body.Data.InstitutionId);
            Assert.Equal(2, body.Data.Topics.Count);
            Assert.Equal("Introduction to Algebra", body.Data.Topics[0].Content);
            Assert.Equal(1, body.Data.Topics[0].LessonNumber);
            Assert.Equal("Calculus Fundamentals", body.Data.Topics[1].Content);
            Assert.Equal(2, body.Data.Topics[1].LessonNumber);
        }

        [Fact]
        public async Task CreateDiscipline_ReturnsNotFound_WhenInstitutionNotFound()
        {
            var response = AthenaApiResponse<CreateDisciplineResponseDto>.NotFound("Institution not found.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateDisciplineCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var dto = new CreateDisciplineDto
            {
                Name = "Physics",
                StudyHours = 60,
                Credits = 4,
                ChargePayment = false
            };

            var result = await _controller.CreateDiscipline(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateDiscipline_ReturnsOk_WhenSuccessful()
        {
            var institution = BuildInstitution();
            var discipline = BuildDiscipline(institution);
            discipline.Name = "Advanced Mathematics";
            discipline.StudyHours = 80;

            var response = AthenaApiResponse<UpdateDisciplineResponseDto>.Ok(new UpdateDisciplineResponseDto(discipline));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateDisciplineCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _controller.UpdateDiscipline(discipline.Id, new UpdateDisciplineDto { Name = "Advanced Mathematics", StudyHours = 80 });

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<UpdateDisciplineResponseDto>>(objectResult.Value);
            Assert.True(body.Success);
            Assert.NotNull(body.Data);
            Assert.Equal(discipline.Id, body.Data.Id);
            Assert.Equal("Advanced Mathematics", body.Data.Name);
            Assert.Equal(80, body.Data.StudyHours);
            Assert.Equal(institution.Id, body.Data.InstitutionId);
        }

        [Fact]
        public async Task UpdateDiscipline_ReturnsNotFound_WhenDisciplineNotFound()
        {
            var response = AthenaApiResponse<UpdateDisciplineResponseDto>.NotFound("Discipline not found.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateDisciplineCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _controller.UpdateDiscipline(Guid.NewGuid(), new UpdateDisciplineDto());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }
    }
}
