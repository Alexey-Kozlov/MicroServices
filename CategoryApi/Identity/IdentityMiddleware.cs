using Microsoft.AspNetCore.Http.Extensions;
using System.Net;

namespace MIdentity
{
    public class IdentityMiddleware : IMiddleware
    {
        private readonly IIdentityService _identityService;

        public IdentityMiddleware(IIdentityService identityService)
        {
            _identityService = identityService;
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
                try
                {
                        var principal = _identityService.GetPrincipal(token!);
                        if (principal != null)
                        {
                            //валидный токен, получен пользователь
                            context.User = principal;
                        }

                }
                catch (UnauthorizedAccessException)
                {
                    //ошибка с токеном - не валидный
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

            }
            await next(context);
        }
    }
}
