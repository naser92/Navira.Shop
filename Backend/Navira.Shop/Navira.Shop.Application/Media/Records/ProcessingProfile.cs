namespace Navira.Shop.Application.Media
{

    public sealed record ProcessingProfile(
        string Name,
        IReadOnlyList<TargetProfile> Targets);
}
