namespace Navira.Shop.Core.Results
{
    [Serializable]
    public class Result : IResult
    {
        public Result() { }

        public bool Error { get; set; }

        public string Message { get; set; }
        public Guid? TraceId { get; set; }
        public string Code { get; set; }


        public static IResult Fail()
        {
            return new Result { Error = true };
        }

        public static IResult Fail(string message)
        {
            return new Result { Error = true, Message = message };
        }
        public static IResult Fail(string message, string code)
        {
            return new Result { Error = true, Message = message, Code = code };
        }
        public static Task<IResult> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static Task<IResult> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }
        public static Task<IResult> FailAsync(string message, string code)
        {
            return Task.FromResult(Fail(message, code));
        }
        public static IResult Success()
        {
            return new Result { Error = false };
        }

        public static IResult Success(string message)
        {
            return new Result { Error = false, Message = message };
        }

        public static Task<IResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }
    }

    [Serializable]
    public class Result<T> : Result, IResult<T>
    {
        public Result() { }

        public T Data { get; set; }

        public static IResult<T> Fail(T data)
        {
            return new Result<T> { Error = true, Data = data };
        }

        public static IResult<T> Fail(string message, T data)
        {
            return new Result<T> { Error = true, Message = message, Data = data };
        }
        public static IResult<T> Fail(string message, T data, string code)
        {
            return new Result<T> { Error = true, Message = message, Data = data, Code = code };
        }

        public static Task<IResult<T>> FailAsync(T data)
        {
            return Task.FromResult(Fail(data));
        }

        public static Task<IResult<T>> FailAsync(string message, T data)
        {
            return Task.FromResult(Fail(message, data));
        }
        public static Task<IResult<T>> FailAsync(string message, T data, string code)
        {
            return Task.FromResult(Fail(message, data, code));
        }


        public static IResult<T> Success(T data)
        {
            return new Result<T> { Error = false, Data = data };
        }

        public static IResult<T> Success(T data, string message)
        {
            return new Result<T> { Error = false, Data = data, Message = message };
        }


        public static Task<IResult<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<IResult<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }
    }
}
