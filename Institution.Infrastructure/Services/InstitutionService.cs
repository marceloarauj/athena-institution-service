using Institution.Application.Interfaces.Services;
using Institution.Application.Options;
using Institution.Infrastructure.Contexts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Institution.Infrastructure.Services
{
    public class InstitutionService
    (
        InstitutionContext institutionContext,
        IAmazonService amazonService,
        IOptions<AmazonS3Options> options
    ) : IInstitutionService
    {
        private readonly AmazonS3Options _options = options.Value;
        private readonly InstitutionContext _institutionContext = institutionContext;
        private readonly IAmazonService _amazonService = amazonService;

        public async Task UpdateInstitutionLogo(IFormFile file)
        {
            if (_options.Bucket?.Institutions == null)
                throw new InvalidOperationException("Bucket name is not configured.");

            var bucketName = _options.Bucket.Institutions;

            var extension = Path.GetExtension(file.FileName);

            var fileName = $"logo{extension}";

            var key = $"{_institutionContext.Alias}/logo/{fileName}";

            await _amazonService.UploadFile(key, bucketName, file);
        }
    }
}