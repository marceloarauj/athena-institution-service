namespace Institution.Application.Options
{
    public class AmazonS3Options
    {
        public string? ServiceUrl { get; set; }
        public string? AccessKey { get; set; }
        public string? Secret { get; set; }
        public string? Region { get; set; }
        public BucketOptions? Bucket { get; set; }
    }
}
