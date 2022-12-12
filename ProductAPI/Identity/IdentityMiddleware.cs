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
                //токен есть, но если это обращение к фронту - пропускаем, для фронта контекст пользователя не нужен
                if (!context.Request.GetDisplayUrl().Contains(_config["FrontUrl"]!))
                {
                    var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
                    try
                    {
                        if (await _identityService.CheckToken(token))
                        {
                            var principal = _identityService.GetPrincipal(token!);
                            if (principal != null)
                            {
                                //валидный токен, получен пользователь
                                context.User = principal;
                            }
                        }
                        else
                        {
                            //ошибка с токеном - не валидный
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        //ошибка с токеном - не валидный
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
            }
            await next(context);
        }
    }
}
