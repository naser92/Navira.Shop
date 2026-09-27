namespace Navira.Shop.Application.Media
{
    public sealed record PreparedImageMetadata(
    string ContentType,
    int Width,
    int Height,
    long SizeBytes,
    string ProcessingProfile,
    int ProcessingProfileVersion,
    string EditSettingsJson);
}
