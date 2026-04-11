using Microsoft.AspNetCore.Http;

namespace Institution.Application.Interfaces.Services
{
    public interface IInstitutionService
    {
        public Task UpdateInstitutionLogo(IFormFile file);
    }
}