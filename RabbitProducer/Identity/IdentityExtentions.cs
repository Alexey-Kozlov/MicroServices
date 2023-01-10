namespace MIdentity
{
    public static class IdentityExtentions
    {
        public static IServiceCollection AddMIdentity(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(IdentityMapping));
            builder.Services.Configure<IdentitySettings>(builder.Configuration);
            builder.Services.AddScoped<IdentityMiddleware>();
            builder.Services.AddHttpClient<IIdentityService, IdentityService>();
            builder.Services.AddScoped<IIdentityService, IdentityService>();
            return services;
        }
    }
}
