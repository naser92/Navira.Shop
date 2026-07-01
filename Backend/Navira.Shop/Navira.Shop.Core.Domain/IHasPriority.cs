namespace Navira.Shop.Core.Domain
{
    /// <summary>
    /// It's a marker interface, in order to make our entities prioritised.
    /// Every entity you mark with this interface, will save audit info to the database.
    /// More info: http://www.dotnettips.info/post/2577
    /// and http://www.dotnettips.info/post/2578
    /// </summary>
    public interface IHasPriority
    {
        int Priority { get; set; }
    }
}