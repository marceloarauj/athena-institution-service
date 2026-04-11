using Institution.Application.Auth;
using Institution.Application.Commands;
using Institution.Application.Dtos.Input;
using Institution.Application.Handlers;
using Institution.Application.Interfaces;
using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Moq;

namespace Institution.Tests.Integration
{
    public class DisciplineHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IDisciplineRepository> _disciplineRepoMock = new();
        private readonly Mock<IDisciplineUpdateHistoryRepository> _historyRepoMock = new();
        private readonly Mock<IUser> _userMock = new();
        private readonly Guid _userId = Guid.NewGuid();
        private readonly UpdateDisciplineHandler _handler;

        public DisciplineHandlerTests()
        {
            _unitOfWorkMock.Setup(u => u.DisciplineRepository).Returns(_disciplineRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.DisciplineUpdateHistoryRepository).Returns(_historyRepoMock.Object);
            _userMock.Setup(u => u.UserId).Returns(_userId);
            _handler = new UpdateDisciplineHandler(_unitOfWorkMock.Object, _userMock.Object);
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

        [Fact]
        public async Task UpdateDiscipline_SavesHistoryForChangedFields()
        {
            var institution = BuildInstitution();
            var discipline = BuildDiscipline(institution);

            _disciplineRepoMock
                .Setup(r => r.FindByIdAsync(discipline.Id))
                .ReturnsAsync(discipline);

            var dto = new UpdateDisciplineDto { Name = "Advanced Mathematics", StudyHours = 80 };

            var result = await _handler.Handle(new UpdateDisciplineCommand(discipline.Id, dto), CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal("Advanced Mathematics", result.Data.Name);
            Assert.Equal(80, result.Data.StudyHours);

            _historyRepoMock.Verify(r => r.AddRangeAsync(
                It.Is<List<DisciplineUpdateHistoryEntity>>(list =>
                    list.Count == 2 &&
                    list.Any(h => h.Field == "Name" && h.OldValue == "Mathematics" && h.NewValue == "Advanced Mathematics" && h.UpdatedByUser == _userId) &&
                    list.Any(h => h.Field == "StudyHours" && h.OldValue == "60" && h.NewValue == "80" && h.UpdatedByUser == _userId)
                )
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateDiscipline_DoesNotSaveHistory_WhenNoFieldsChanged()
        {
            var institution = BuildInstitution();
            var discipline = BuildDiscipline(institution);

            _disciplineRepoMock
                .Setup(r => r.FindByIdAsync(discipline.Id))
                .ReturnsAsync(discipline);

            var dto = new UpdateDisciplineDto(); // all null — no changes

            var result = await _handler.Handle(new UpdateDisciplineCommand(discipline.Id, dto), CancellationToken.None);

            Assert.True(result.Success);
            _historyRepoMock.Verify(r => r.AddRangeAsync(It.IsAny<List<DisciplineUpdateHistoryEntity>>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDiscipline_SavesHistoryOnlyForChangedFields_WhenPartialUpdate()
        {
            var institution = BuildInstitution();
            var discipline = BuildDiscipline(institution);

            _disciplineRepoMock
                .Setup(r => r.FindByIdAsync(discipline.Id))
                .ReturnsAsync(discipline);

            var dto = new UpdateDisciplineDto { Credits = 6 }; // only Credits changes

            var result = await _handler.Handle(new UpdateDisciplineCommand(discipline.Id, dto), CancellationToken.None);

            Assert.True(result.Success);

            _historyRepoMock.Verify(r => r.AddRangeAsync(
                It.Is<List<DisciplineUpdateHistoryEntity>>(list =>
                    list.Count == 1 &&
                    list[0].Field == "Credits" &&
                    list[0].OldValue == "4" &&
                    list[0].NewValue == "6" &&
                    list[0].UpdatedByUser == _userId
                )
            ), Times.Once);
        }

        [Fact]
        public async Task UpdateDiscipline_ReturnsNotFound_WhenDisciplineNotFound()
        {
            _disciplineRepoMock
                .Setup(r => r.FindByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((DisciplineEntity?)null);

            var result = await _handler.Handle(new UpdateDisciplineCommand(Guid.NewGuid(), new UpdateDisciplineDto()), CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            _historyRepoMock.Verify(r => r.AddRangeAsync(It.IsAny<List<DisciplineUpdateHistoryEntity>>()), Times.Never);
        }
    }
}
