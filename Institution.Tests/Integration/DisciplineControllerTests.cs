using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Institution.Controllers;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Institution.Infrastructure.Contexts.Models;
using Mediator.Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Institution.Tests.Integration
{
    public class DisciplineControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly InstitutionContext _institutionContext = new() { Alias = "test-institution" };
        private readonly DisciplineController _controller;

        public DisciplineControllerTests()
        {
            _controller = new DisciplineController(_mediatorMock.Object, _institutionContext);
        }

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

        private static InstitutionEntity BuildInstitution() => new()
        {
            Id = Guid.NewGuid(),
            Alias = "test-institution",
            DisplayName = "Test Institution",
            ChargePayment = false,
            PaymentFormat = PaymentFormat.Free
        };

        [Fact]
        public async Task CreateDiscipline_ReturnsCreated_WhenSuccessful()
        {
            var institution = BuildInstitution();
            var discipline = BuildDiscipline(institution);
            var response = AthenaApiResponse<CreateDisciplineResponseDto>.Created(new CreateDisciplineResponseDto(discipline));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateDisciplineCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var dto = new CreateDisciplineDto
            {
                Name = discipline.Name,
                StudyHours = discipline.StudyHours,
                Credits = discipline.Credits,
                ChargePayment = discipline.ChargePayment
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
    }
}
