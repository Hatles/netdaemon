using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetDaemon.Common;

namespace NetDaemon.Http;

public static class RuntimeMiddlewareExtensions
{
    public static IServiceCollection AddRuntimeMiddleware(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.AddSingleton<RuntimeMiddlewareService>();
        services.AddScoped<AppMiddlewareBuilder>(provider => new AppMiddlewareBuilder(provider.GetRequiredService<RuntimeMiddlewareService>(), provider.GetRequiredService<IApplicationContext>()));
        return services;
    }

    public static IApplicationBuilder UseRuntimeMiddleware(this IApplicationBuilder app, Action<IApplicationBuilder> defaultAction = null)
    {
        var service = app.ApplicationServices.GetRequiredService<RuntimeMiddlewareService>();
        service.Use(app);
        if (defaultAction != null)
        {
            service.Configure(defaultAction);
        }
        return app;
    }
}