using Microsoft.AspNetCore.Http.Extensions;
using System.Net;

namespace MIdentity
{
    public class IdentityMiddleware : IMiddleware
    {
        private readonly IConfiguration _config;
        private readonly IIdentityService _identityService;
        private readonly ILogger<IdentityMiddleware> _logger;

        public IdentityMiddleware(IIdentityService identityService,
            IConfiguration config,
            ILogger<IdentityMiddleware> logger)
        {
            _identityService = identityService;
            _config = config;
            _logger = logger;
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
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
                try
                {
                    if (await _identityService.CheckToken(token))
                    {
                        var principal = _identityService.GetPrincipal(token!);
                        if (principal != null)
                        {
                            _logger.LogInformation("валидный токен - " + token);
                            //валидный токен, получен пользователь
                            context.User = principal;
                        }
                    }
                    else
                    {
                        //ошибка с токеном - не валидный
                        _logger.LogInformation("Ошибочный токен - " + token);
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    //ошибка с токеном - не валидный
                    _logger.LogInformation("Ошибка с токеном - " + token);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
            await next(context);
        }
    }
}
