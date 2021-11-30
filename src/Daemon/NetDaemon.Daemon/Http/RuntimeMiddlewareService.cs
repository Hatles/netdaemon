using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace NetDaemon.Http;

internal class RuntimeMiddlewareService
{
    private Func<RequestDelegate, RequestDelegate>? _middleware;

    private IApplicationBuilder _appBuilder;

    internal void Use(IApplicationBuilder app)
    {
        _appBuilder = app.Use(next => context =>
        {
            if (_middleware == null)
            {
                return next(context);
            }

            return _middleware(next)(context);
        });
    }

    public Dictionary<MiddlewareConfiguration, Func<RequestDelegate, RequestDelegate>> _middlewares { get; set; } = new();

    internal MiddlewareConfiguration Configure(Action<IApplicationBuilder> action)
    {
        var app = _appBuilder.New();
        action(app);
        return RegisterDelegate(next => app.Use(_ => next).Build());
    }

    private MiddlewareConfiguration RegisterDelegate(Func<RequestDelegate, RequestDelegate> middlewareDelegate)
    {
        var configuration = new MiddlewareConfiguration(this);
        _middlewares.Add(configuration, middlewareDelegate);

        UpdateMiddlewareStack();
        
        return configuration;
    }

    private void UpdateMiddlewareStack()
    {
        var stackApp = _appBuilder.New();

        _middlewares.ToList().ForEach(middleware =>
        {
            stackApp.Use(next => context => middleware.Value(next)(context));
        });

        _middleware = next => stackApp.Use(_ => next).Build();
    }

    internal void Clear()
    {
        _middleware = null;
        _middlewares.Clear();
    }

    internal void UnConfigure(MiddlewareConfiguration middlewareConfiguration)
    {
        if (_middlewares.ContainsKey(middlewareConfiguration))
        {
            _middlewares.Remove(middlewareConfiguration);
            UpdateMiddlewareStack();
        }
    }
    
    internal MiddlewareConfiguration ConfigureApp(string applicationId, Action<IApplicationBuilder> action)
    {
        var app = _appBuilder.New().Map("/apps/" + applicationId, action);
        return RegisterDelegate(next => app.Use(_ => next).Build());
    }
}

public class MiddlewareConfiguration {
    private readonly RuntimeMiddlewareService _runtimeMiddlewareService;
    private bool _configured = true;

    public bool Configured
    {
        get => _configured;
    }

    internal MiddlewareConfiguration(RuntimeMiddlewareService runtimeMiddlewareService)
    {
        _runtimeMiddlewareService = runtimeMiddlewareService;
    }
    
    public void UnConfigure()
    {
        if (_configured)
        {
            _runtimeMiddlewareService.UnConfigure(this);
            _configured = false;
        }
    }
}