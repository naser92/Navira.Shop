namespace Navira.Shop.Core.Results
{
    public interface IResult
    {
        bool Error { get; }
        string Code { get; }

        string Message { get; }
        Guid? TraceId { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}
