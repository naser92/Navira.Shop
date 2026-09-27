namespace Navira.Shop.Application.Media
{
    public sealed record StoredObject(
    string Bucket,
    string ObjectKey,
    string ContentType,
    long SizeBytes);
}
