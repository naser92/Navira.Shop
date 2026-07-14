using Microsoft.AspNetCore.Mvc;
using Navira.Shop.Core.Results;

namespace Navira.Shop.Core.Web
{
    public static class ResultExtensions
    {
        public static async Task<IActionResult> ApiResultAsync<T>(this Task<IResult<T>> result) => ActionResult.Create(await result.ConfigureAwait(false));

        public static async Task<IActionResult> ApiResultAsync(this Task<IResult> result) => ActionResult.Create(await result.ConfigureAwait(false));
        public static IActionResult ApiResult(this IResult result) => ActionResult.Create(result);
        public static IActionResult SuccessApiResult<T>(this T data) => ActionResult.Success(data);

        public static IActionResult FailApiResult<T>(this T data) => ActionResult.Fail(data);
        public static IActionResult SuccessApiResult<T>(this T data, string message) => ActionResult.Success(data, message);

        public static IActionResult FailApiResult<T>(this T data, string message) => ActionResult.Fail(data, message);
        public static IActionResult SuccessApiResult<T>(this string message) => ActionResult.Success(message);

        public static IActionResult FailApiResult<T>(this string message) => ActionResult.Fail(message);
    }
}
