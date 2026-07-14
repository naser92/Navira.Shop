using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Core.Web
{
    public class BaseActionResult : IActionResult
    {
        private readonly IResult _result;

        protected BaseActionResult(IResult result)
        {
            _result = result;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            object value = null;

            if (_result.Error)
            {
                value = _result.Message;
            }

            if (_result.GetType().IsGenericType && _result.GetType().GetGenericTypeDefinition() == typeof(Result<>))
            {
                value = (_result as dynamic)?.Data;
            }

            var objectResult = new ObjectResult(_result)
            {
                StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status200OK
            };

            return objectResult.ExecuteResultAsync(context);
        }

    }

    public sealed class ActionResult : BaseActionResult
    {

        protected ActionResult(IResult result) : base(result)
        {
        }

        //private ApiResult(object data)
        //{
        //    _result = Result<object>.Success(data);
        //}
        //private ApiResult(object data,bool hasError)
        //{
        //    _result = hasError ? data.FailResult() : data.SuccessResult();
        //}
        //private ApiResult(object data,string message, bool hasError)
        //{
        //    _result = hasError ? data.FailResult(message) : data.SuccessResult(message);
        //}
        //private ApiResult(string message, bool hasError)
        //{
        //    _result = hasError? message.FailResult():message.SuccessResult();
        //}

        public static IActionResult Create(IResult result)
        {
            return new ActionResult(result);
        }

        public static IActionResult Success<T>(T data)
        {
            return new ActionResult(data.SuccessResult());
        }

        public static IActionResult Success<T>(T data, string message)
        {
            return new ActionResult(data.SuccessResult(message));
        }
        public static IActionResult Success(string message)
        {
            return new ActionResult(message.SuccessResult());
        }

        public static IActionResult Fail<T>(T data)
        {
            return new ActionResult(data.FailResult());
        }

        public static IActionResult Fail<T>(T data, string message)
        {
            return new ActionResult(data.FailResult(message));
        }
        public static IActionResult Fail(string message)
        {
            return new ActionResult(message.FailResult());
        }

    }

    public sealed class SuccessResult : BaseActionResult
    {
        public SuccessResult() : base(Result.Success())
        {
        }
    }

    public sealed class FailResult : BaseActionResult
    {
        public FailResult() : base(Result.Fail())
        {
        }
    }
}
