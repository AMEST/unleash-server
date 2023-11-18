using Microsoft.AspNetCore.Authentication.Cookies;
using UnleashServer.Json;
using Skidbladnir.Modules;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace UnleashServer.Web.Host.Services;

public class AspNetModule : Module
{
    public override void Configure(IServiceCollection services)
    {
        services.AddHttpContextAccessor()
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "UnleashServer";
                options.Events.OnRedirectToAccessDenied = c =>
                {
                    c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.FromResult<object>(null);
                };
            })
            .AddOpenIdConnect(options =>
            {
                Configuration.AppConfiguration.GetSection("Openid").Bind(options);
            });
        services.AddControllers()
            .AddJsonOptions(x => x.JsonSerializerOptions.ApplyDefaults());
        services.AddDataProtection()
            .SetApplicationName("UnleashServer");
        services.AddSwaggerGen(options =>
        {
            options.UseOneOfForPolymorphism();
            options.SelectDiscriminatorNameUsing(_ => "$type");
            options.SelectDiscriminatorValueUsing(t => t.Name);

            options.MapType<TimeSpan>(() => new OpenApiSchema()
            {
                Type = "string",
                Example = new OpenApiString("02:00:00")
            });
        });

        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });
    }
}