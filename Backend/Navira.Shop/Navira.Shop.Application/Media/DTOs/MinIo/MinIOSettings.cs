namespace Navira.Shop.Application.Media
{
    public sealed class MinIOSettings
    {
        public string Endpoint { get; init; } = default!;
        public string AccessKey { get; init; } = default!;
        public string SecretKey { get; init; } = default!;
        public string BucketName { get; init; } = default!;
        public bool UseSSL { get; init; }
    }
}
