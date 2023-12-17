using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using System.Web;
using BasicCrudAPI.Annotations;
using BasicCrudAPI.Classes;
using BasicCrudAPI.Models.AdminModels;

namespace BasicCrudAPI.Middleware
{
    public class AuthenticationFilter : IActionFilter, IOrderedFilter
    {
        public DbHelper _dbHelper;

        public AuthenticationFilter(IConfiguration config)
        {
            _dbHelper = new DbHelper(config);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //get the method that we're calling 
            var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            MethodInfo method = context.Controller.GetType().GetMethod(actionName);
            if (method != null)
            {
                //check if the method is decorated with an allow anonymous attribute so we dont require auth
                if (method.GetCustomAttribute<AllowAnonymousAttribute>() != null)
                {
                    return;
                }

                //if we do grab the auth header
                var headers = context.HttpContext.Request.Headers;
                var headerExists = headers.TryGetValue("Authentication", out var authorizationHeader);

                //if they dont have an authorization header immediately log them out
                if (!headerExists || string.IsNullOrWhiteSpace(authorizationHeader))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                var isAdminEndpoint = context.Controller.GetType().GetCustomAttribute<AdminAttribute>() != null || method.GetCustomAttribute<AdminAttribute>() != null; 
                                      
                
                //check if user is authenticated and admin if admin is true
                //if not return new UnauthorizedResult()
                var url = context.HttpContext.Request.GetDisplayUrl();
                var body = "";
                if (context.ActionArguments.Any(x => x.Key == "record"))
                {
                    body = JsonSerializer.Serialize(context.ActionArguments["record"], new JsonSerializerOptions()
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    });
                }
                if (!AuthenticateRequest(url, body, authorizationHeader, out var userId))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var user = _dbHelper.SelectRecord<User>(userId);
                if (isAdminEndpoint && !user.IsAdmin)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                if (!user.IsApproved)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }

        public bool AuthenticateRequest(string url, string postBody, string authorizationHeaderValue, out string userId)
        {
            try
            {
                var actualString =
                    System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authorizationHeaderValue));
                var encodingDateString = actualString.Split('|').First();
                userId = actualString.Split('|')[1];
                var comparisonValue = actualString.Split('|').Last();
                var copyOfUserId = userId;
                var existingToken = _dbHelper.GetRecords<Token>(x => x.UserId == copyOfUserId).First();
                var encodingDate = DateTime.Parse(encodingDateString ).ToUniversalTime();
                if (encodingDate < DateTime.UtcNow.AddMinutes(-15))
                {
                    return false;
                }
                var dateString = encodingDate.ToString("O");
                var key = $"{dateString}{existingToken.AuthorizationToken}";
                var hashString = $"{url}{postBody}";
                using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
                {
                    var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(hashString));
                    var generatedCompareValue = Convert.ToBase64String(hash);
                    return generatedCompareValue  == comparisonValue;
                }
            }
            catch (Exception ex)
            {
                userId = "";
                return false;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public int Order { get; } = 1;
    }
}
