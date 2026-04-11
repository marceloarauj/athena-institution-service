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
    public class InstitutionControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly InstitutionController _controller;

        public InstitutionControllerTests()
        {
            _controller = new InstitutionController(_mediatorMock.Object);
        }

        private static InstitutionEntity BuildInstitution() => new()
        {
            Id = Guid.NewGuid(),
            Alias = "test-institution",
            DisplayName = "Test Institution",
            ChargePayment = false,
            PaymentFormat = PaymentFormat.Free
        };

        [Fact]
        public async Task CreateInstitution_ReturnsCreated_WhenSuccessful()
        {
            var institution = BuildInstitution();
            var response = AthenaApiResponse<CreateInstitutionResponseDto>.Created(new CreateInstitutionResponseDto(institution));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateInstitutionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var dto = new CreateInstitutionDto
            {
                Alias = institution.Alias,
                DisplayName = institution.DisplayName,
                IsPrivate = false,
                ChargePayment = false,
                PaymentFormat = PaymentFormat.Free
            };

            var result = await _controller.CreateInstitution(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<CreateInstitutionResponseDto>>(objectResult.Value);
            Assert.True(body.Success);
            Assert.NotNull(body.Data);
            Assert.Equal(institution.Alias, body.Data.Alias);
            Assert.Equal(institution.DisplayName, body.Data.DisplayName);
            Assert.Equal(institution.ChargePayment, body.Data.ChargePayment);
            Assert.Equal(institution.PaymentFormat, body.Data.PaymentFormat);
        }

        [Fact]
        public async Task CreateInstitution_ReturnsUnprocessableEntity_WhenAliasAlreadyExists()
        {
            var response = AthenaApiResponse<CreateInstitutionResponseDto>.UnprocessableEntity("Institution alias already exists.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateInstitutionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var dto = new CreateInstitutionDto
            {
                Alias = "existing-alias",
                DisplayName = "Test",
                IsPrivate = false,
                ChargePayment = false,
                PaymentFormat = PaymentFormat.Free
            };

            var result = await _controller.CreateInstitution(dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(422, objectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateInstitution_ReturnsOk_WhenSuccessful()
        {
            var institution = BuildInstitution();
            var response = AthenaApiResponse<UpdateInstitutionResponseDto>.Ok(new UpdateInstitutionResponseDto(institution));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInstitutionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var dto = new UpdateInstitutionDto { DisplayName = "Updated Name" };

            var result = await _controller.UpdateInstitution(institution.Alias, dto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var body = Assert.IsType<AthenaApiResponse<UpdateInstitutionResponseDto>>(objectResult.Value);
            Assert.True(body.Success);
            Assert.NotNull(body.Data);
            Assert.Equal(institution.Alias, body.Data.Alias);
            Assert.Equal(institution.DisplayName, body.Data.DisplayName);
            Assert.Equal(institution.ChargePayment, body.Data.ChargePayment);
            Assert.Equal(institution.PaymentFormat, body.Data.PaymentFormat);
        }

        [Fact]
        public async Task UpdateInstitution_ReturnsNotFound_WhenAliasNotFound()
        {
            var response = AthenaApiResponse<UpdateInstitutionResponseDto>.NotFound("Institution not found.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateInstitutionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _controller.UpdateInstitution("unknown-alias", new UpdateInstitutionDto());

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }
    }
}
