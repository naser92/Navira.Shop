namespace Navira.Shop.Core.Results
{
    [Serializable]
    public class ResultException : Exception
    {
        public Task<IResult> Result { get; }
        public ResultException(string message)
            : base(message)
        {
            Result = Results.Result.FailAsync(message);
        }
        public ResultException(string message, Exception inner)
            : base(message, inner)
        {
            Result = Results.Result.FailAsync(message);
        }
        public ResultException(string message, string Code = "") : base(message)
        {
            Result = Results.Result.FailAsync(message, Code);
        }
    }
}
