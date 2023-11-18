using Skidbladnir.Modules;
using UnleashServer.Web.Host.Services;

namespace UnleashServer.Web.Host;

public class StartupModule : Module
{
    public override Type[] DependsModules => ModulesBuilder.Create()
        .Add<AspNetModule>()
        .Build();
}