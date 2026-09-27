namespace Navira.Shop.Application.Media
{
    public sealed record PreparedImageVariant(
    string Profile,
    string ContentType,
    string Extension,
    int Width,
    int Height,
    long SizeBytes,
    Stream Content);
}
