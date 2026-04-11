using Institution.Application.Dtos.Input;

namespace Institution.Application.Interfaces.Services
{
    public interface IGradeService
    {
        public Task<bool> IsApproved(List<TestGradeDto> grades, string formula);
    }
}
