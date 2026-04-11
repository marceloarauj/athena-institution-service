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
    public class EventControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _controller = new EventController(_mediatorMock.Object);
        }

        private static InstitutionEntity BuildInstitution() => new()
        {
            Id = Guid.NewGuid(),
            Alias = "test-institution",
            DisplayName = "Test Institution",
            ChargePayment = false,
            PaymentFormat = PaymentFormat.Free
        };

        private static EventEntity BuildEvent(InstitutionEntity institution) => new()
        {
            Id = Guid.NewGuid(),
            Name = "Academic Week",
            Description = "Annual academic event",
            StartDate = new DateTime(2026, 5, 1, 9, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2026, 5, 5, 18, 0, 0, DateTimeKind.Utc),
            Institution = institution,
            InstitutionId = institution.Id
        };

        [Fact]
        public async Task CreateEvent_ReturnsCreated_WhenSuccessful()
        {
            var institution = BuildInstitution();
            var @event = BuildEvent(institution);
            var response = AthenaApiResponse<CreateEventResponseDto>.Created(new CreateEventResponseDto(@event));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateEventCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var dto = new CreateEventDto
            {
                Name = @event.Name,
                Description = @event.Description,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate
            };

            var result = await _controller.CreateEvent(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<CreateEventResponseDto>>(objectResult.Value);
            Assert.True(body.Success);
            Assert.NotNull(body.Data);
            Assert.Equal(@event.Id, body.Data.Id);
            Assert.Equal(@event.Name, body.Data.Name);
            Assert.Equal(@event.Description, body.Data.Description);
            Assert.Equal(@event.StartDate, body.Data.StartDate);
            Assert.Equal(@event.EndDate, body.Data.EndDate);
            Assert.Equal(institution.Id, body.Data.InstitutionId);
        }

        [Fact]
        public async Task CreateEvent_ReturnsUnprocessableEntity_WhenEndDateBeforeStartDate()
        {
            var response = AthenaApiResponse<CreateEventResponseDto>.UnprocessableEntity("End date must be after start date.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateEventCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var dto = new CreateEventDto
            {
                Name = "Invalid Event",
                StartDate = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            var result = await _controller.CreateEvent(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateEvent_ReturnsNotFound_WhenInstitutionNotFound()
        {
            var response = AthenaApiResponse<CreateEventResponseDto>.NotFound("Institution not found.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateEventCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var dto = new CreateEventDto
            {
                Name = "Some Event",
                StartDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc)
            };

            var result = await _controller.CreateEvent(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async Task ListEvents_ReturnsOk_WithEvents()
        {
            var institution = BuildInstitution();
            var @event = BuildEvent(institution);
            var events = new List<EventResponseDto> { new(@event) };
            var response = AthenaApiResponse<List<EventResponseDto>>.Ok(events);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ListEventsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _controller.ListEvents(new ListEventsFilterDto());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<List<EventResponseDto>>>(objectResult.Value);
            
            Assert.True(body.Success);
            Assert.NotNull(body.Data);
            Assert.Single(body.Data);
            Assert.Equal(@event.Id, body.Data[0].Id);
            Assert.Equal(@event.Name, body.Data[0].Name);
            Assert.Equal(@event.Description, body.Data[0].Description);
            Assert.Equal(@event.StartDate, body.Data[0].StartDate);
            Assert.Equal(@event.EndDate, body.Data[0].EndDate);
        }

        [Fact]
        public async Task ListEvents_ReturnsNotFound_WhenInstitutionNotFound()
        {
            var response = AthenaApiResponse<List<EventResponseDto>>.NotFound("Institution not found.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ListEventsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _controller.ListEvents(new ListEventsFilterDto());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }
    }
}
