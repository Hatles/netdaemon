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
public class StaticFilesApp
{
    public StaticFilesApp(AppMiddlewareBuilder appMiddlewareBuilder)
    {
        var appsAssembly = typeof(StaticFilesApp).GetTypeInfo().Assembly;

        var test = appsAssembly.GetManifestResourceNames().ToList();
        
        var dependenciesAppEmbeddedFileProvider = new EmbeddedFileProvider(
            appsAssembly,
            "NetDaemon.DevelopmentApps.apps.Dependencies.ClientApp"
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