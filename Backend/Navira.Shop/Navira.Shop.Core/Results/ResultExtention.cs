namespace Navira.Shop.Core.Results
{
    public static class ResultExtention
    {

        public static async Task<IResult<T>> ResultAsync<T>(this Task<T> result)
        {
            return (await result.ConfigureAwait(false)).SuccessResult();
        }

        public static async Task<IResult<T>> ResultAsync<T>(this Task<T> result, string message)
        {
            return (await result.ConfigureAwait(false)).SuccessResult(message);

        }

        public static IResult<T> SuccessResult<T>(this T data)
        {
            return Result<T>.Success(data);

        }
        public static IResult<T> SuccessResult<T>(this T data, string message)
        {
            return Result<T>.Success(data, message);

        }
        public static IResult SuccessResult(this string message)
        {
            return Result.Success(message);

        }
        public static IResult<T> FailResult<T>(this T data)
        {
            return Result<T>.Fail(data);

        }
        public static IResult<T> FailResult<T>(this T data, string message)
        {
            return Result<T>.Fail(message, data);

        }
        public static IResult<T> FailResult<T>(this T data, string message, string code)
        {
            return Result<T>.Fail(message, data, code);

        }

        public static IResult FailResult(this string message)
        {
            return Result.Fail(message);

        }
        public static IResult FailResult(this string message, string code)
        {
            return Result.Fail(message, code);

        }

    }
}
