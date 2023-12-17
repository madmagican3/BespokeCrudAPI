using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BasicCrudAPI.Middleware
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                context.Result = new ObjectResult(JsonSerializer.Serialize(new Error()
                {
                    Message = "An Error has occurred, please contact someone"
                }));
                context.Exception = null;
            }
        }

        public int Order { get; } = int.MaxValue - 10;
    }

    public class Error
    {
        public string Message { get; set; }
        public DateTime OccurenceDate = DateTime.UtcNow;
    }
}
