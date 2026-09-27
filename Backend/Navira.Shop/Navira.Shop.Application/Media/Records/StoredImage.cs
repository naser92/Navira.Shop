

namespace Navira.Shop.Application.Media
{
    public sealed record StoredImage(
    StoredObject Original,
    IReadOnlyList<StoredImageVariant> Variants);
}
