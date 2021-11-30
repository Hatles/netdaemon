using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using NetDaemon.Common;
using NetDaemon.Http;

namespace NetDaemon.DevelopmentApps.apps.Dependencies;

[NetDaemonApp]
[Focus]
public class StaticFiles2App
{
    public StaticFiles2App(AppMiddlewareBuilder appMiddlewareBuilder)
    {
        var appsAssembly = typeof(StaticFiles2App).GetTypeInfo().Assembly;

        var test = appsAssembly.GetManifestResourceNames().ToList();
        
        var dependenciesAppEmbeddedFileProvider = new EmbeddedFileProvider(
            appsAssembly,
            "NetDaemon.DevelopmentApps.apps.Dependencies.ClientApp2"
        );
        
        appMiddlewareBuilder.Configure(builder =>
        {
            builder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = dependenciesAppEmbeddedFileProvider,
                RequestPath = "/app"
            });
        });
    }
}