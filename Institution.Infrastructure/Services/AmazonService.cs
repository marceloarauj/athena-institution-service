using Amazon.S3;
using Amazon.S3.Model;
using Institution.Application.Interfaces.Services;
using Institution.Application.Models;
using Institution.Application.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Institution.Infrastructure.Services
{
    public class AmazonService
    (
        IOptions<AmazonS3Options> options
    ) : IAmazonService
    {
        private readonly AmazonS3Options _options = options.Value;

        public async Task<AmazonModel?> GetFile(string key, string bucket)
        {
            var client = CreateClient();

            var response = await client.GetObjectAsync(bucket, key);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return null;

            return new AmazonModel
            {
                File = response.ResponseStream,
                ContentType = response.Headers.ContentType
            };
        }
        
        public async Task UploadFile(string key, string bucket, IFormFile file)
        {
            var client = CreateClient();

            using var stream = file.OpenReadStream();

            await client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = bucket,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType,
            });
        }

        private AmazonS3Client CreateClient()
        {
            var config = new AmazonS3Config
            {
                ServiceURL = _options.ServiceUrl,
                ForcePathStyle = true,
                AuthenticationRegion = _options.Region,
                //RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_options.Region)
            };

            return new AmazonS3Client(_options.AccessKey, _options.Secret, config);
        }
    }
}
