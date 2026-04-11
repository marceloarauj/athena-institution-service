using Institution.Application.Models;
using Microsoft.AspNetCore.Http;

namespace Institution.Application.Interfaces.Services
{
    public interface IAmazonService
    {
        Task UploadFile(string key, string bucket, IFormFile file);
        Task<AmazonModel?> GetFile(string key, string bucket);
    }
}
