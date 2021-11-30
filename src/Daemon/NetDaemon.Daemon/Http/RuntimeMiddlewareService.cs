using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace NetDaemon.Http;

internal class RuntimeMiddlewareService
{
    private Func<RequestDelegate, RequestDelegate> _middleware;

    private IApplicationBuilder _appBuilder;

    internal void Use(IApplicationBuilder app) => _appBuilder = app.Use(next => context =>
    {
        if (_middleware == null)
        {
            return next(context);
        }
        
        return _middleware(next)(context);
    });

    internal void Configure(Action<IApplicationBuilder> action)
    {
        var app = _appBuilder.New();
        action(app);
        _middleware = next => app.Use(_ => next).Build();
    }
    
    internal void ConfigureApp(string applicationId, Action<IApplicationBuilder> action)
    {
        var app = _appBuilder.New().Map("/apps/" + applicationId, builder =>
        {
            action(builder);
        });
        
        _middleware = next => app.Use(_ => next).Build();
    }
}