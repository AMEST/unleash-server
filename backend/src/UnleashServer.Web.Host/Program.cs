using Microsoft.AspNetCore.HttpOverrides;
using Skidbladnir.Modules;
using UnleashServer.Web.Host;
using UnleashServer.Web.Host.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSkidbladnirModules<StartupModule>(configuration => {

});

var app = builder.Build();
app.UseResponseCompression();

// Configure the HTTP request pipeline.
var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    RequireHeaderSymmetry = false
};
forwardedHeadersOptions.KnownNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();

app.UseForwardedHeaders(forwardedHeadersOptions);

app.UseSwagger();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseUserValidationMiddleware();

app.MapControllers();

app.Run();
