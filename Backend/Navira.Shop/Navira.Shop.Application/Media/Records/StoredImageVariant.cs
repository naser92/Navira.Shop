namespace Navira.Shop.Application.Media
{
    public sealed record StoredImageVariant(
    string Profile,
    int Width,
    int Height,
    StoredObject File);
}
