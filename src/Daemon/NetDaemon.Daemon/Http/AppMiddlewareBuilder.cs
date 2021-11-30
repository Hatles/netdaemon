using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using NetDaemon.Common;

namespace NetDaemon.Http;

public sealed class AppMiddlewareBuilder : IDisposable
{
    private readonly RuntimeMiddlewareService _runtimeMiddlewareService;
    private readonly IApplicationContext _applicationContext;

    private readonly List<MiddlewareConfiguration> _middlewareConfigurations = new ();

    internal AppMiddlewareBuilder(RuntimeMiddlewareService runtimeMiddlewareService, IApplicationContext applicationContext)
    {
        _runtimeMiddlewareService = runtimeMiddlewareService;
        _applicationContext = applicationContext;
    }

    public MiddlewareConfiguration Configure(Action<IApplicationBuilder> action)
    {
        var configuration = _runtimeMiddlewareService.ConfigureApp(_applicationContext.Id, action);
        _middlewareConfigurations.Add(configuration);
        return configuration;
    }

    public void UnConfigure(MiddlewareConfiguration configuration)
    {
        _runtimeMiddlewareService.UnConfigure(configuration);
    }

    public void Dispose()
    {
        _middlewareConfigurations.ForEach(configuration => configuration.UnConfigure());
        _middlewareConfigurations.Clear();
    }
}
