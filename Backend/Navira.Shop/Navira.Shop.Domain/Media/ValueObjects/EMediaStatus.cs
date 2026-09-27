namespace Navira.Shop.Domain.Media
{
    public enum MediaStatus : int
    {
        Uploading = 0,
        Processing = 1,
        Ready = 2,
        Failed = 3,
        Deleting = 4,
        Deleted = 5
    }
}
