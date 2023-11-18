using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Modules;
using UnleashServer.Web.User;

namespace UnleashServer.Web;

public class WebModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
    }
}