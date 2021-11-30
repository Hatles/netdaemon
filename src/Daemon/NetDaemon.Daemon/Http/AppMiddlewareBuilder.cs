using System;
using Microsoft.AspNetCore.Builder;
using NetDaemon.Common;

namespace NetDaemon.Http;

public class AppMiddlewareBuilder
{
    private readonly RuntimeMiddlewareService _runtimeMiddlewareService;
    private readonly IApplicationContext _applicationContext;

    internal AppMiddlewareBuilder(RuntimeMiddlewareService runtimeMiddlewareService, IApplicationContext applicationContext)
    {
        _runtimeMiddlewareService = runtimeMiddlewareService;
        _applicationContext = applicationContext;
    }

    public void Configure(Action<IApplicationBuilder> action)
    {
        _runtimeMiddlewareService.ConfigureApp(_applicationContext.Id, action);
    }
}