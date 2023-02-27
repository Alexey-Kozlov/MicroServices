using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;

namespace MIdentity
{
    public class IdentityMiddleware : IMiddleware
    {
        private readonly IConfiguration _config;
        private readonly IIdentityService _identityService;

        public IdentityMiddleware(IIdentityService identityService, IConfiguration config)
        {
            _identityService = identityService;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //Здесь кастомная аутентификация - через отдельный сервис аутентификации
            //Получаем токен из identity и делаем аутентификацию
            if (context.Request.Method != "OPTIONS")
            {
                if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
                var principal = _identityService.GetPrincipal(token!);
                if (principal != null)
                {
                    //валидный токен, получен пользователь
                    context.User = principal;
                }
            }
            await next(context);
        }
    }
}
