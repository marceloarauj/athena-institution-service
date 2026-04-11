using Institution.Application.Dtos.Input;
using Institution.Infrastructure.Services;

namespace Institution.Tests.Unit
{
    public class GradeTests
    {
        private readonly GradeService _gradeService = new();

        [Fact]
        public async Task ShouldReturnApproved()
        {
            var notes = new List<TestGradeDto>
            {
                new() { Key = "N1", Value = 5 },
                new() { Key = "N2", Value = 5 },
                new() { Key = "N3", Value = 5 },
                new() { Key = "N4", Value = 5 }
            };

            var formula = "(N1 + N2 + N3 + N4)/4 >= 5";

            bool result = await _gradeService.IsApproved(notes, formula);

            Assert.True(result);
        }

        [Fact]
        public async Task ShouldReturnReproved()
        {
            var notes = new List<TestGradeDto>
            {
                new() { Key = "N1", Value = 5 },
                new() { Key = "N2", Value = 5 },
                new() { Key = "N3", Value = 5 },
                new() { Key = "N4", Value = 4.9 }
            };

            var formula = "(N1 + N2 + N3 + N4)/4 >= 5";

            bool result = await _gradeService.IsApproved(notes, formula);

            Assert.False(result);
        }
    }
}
