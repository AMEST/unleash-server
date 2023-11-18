namespace UnleashServer.Web.Host.Middlewares;

internal static class MiddlewareExtensions
{
    public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder)
    {
        builder.UseSwagger();
        builder.UseSwaggerUI();
        return builder;
    }


    public static IApplicationBuilder UseUserValidationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ValidateUserStateMiddleware>()
                .MapWhen(
                    ctx => ctx.Request.Path.Value.Equals("/error/account-disabled", StringComparison.OrdinalIgnoreCase),
                    b => b.Run(async context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync(@"
                            <h2>Account has been disabled. Contact with administrators.</h2>
                            <hr>
                            UnleashServer.
                        ");
                    }));
    }
}