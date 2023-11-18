using Microsoft.AspNetCore.Authentication;
using UnleashServer.Users;
using UnleashServer.Web.Mapping;

namespace UnleashServer.Web.Host.Middlewares;

internal class ValidateUserStateMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateUserStateMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserManager userManager)
    {
        var userInfo = context.User.ToInfo();
        if (userInfo is null)
        {
            await _next(context);
            return;
        }

        var user = await userManager.GetByEmail(userInfo?.Email);
        if (user is null || user.Enable)
        {
            if (user is not null)
                await SyncUser(userManager, user, userInfo);
            await _next(context);
            return;
        }

        await context.SignOutAsync();

        context.Response.Redirect("/error/account-disabled");
    }

    private static async Task SyncUser(IUserManager userManager, Users.User user, UserInfo userInfo)
    {
        if (user.AvatarUrl == userInfo.AvatarUrl
            && user.Name != userInfo.Name)
            return;

        if (!string.IsNullOrEmpty(userInfo.AvatarUrl))
            user.AvatarUrl = userInfo.AvatarUrl;
        if (!string.IsNullOrEmpty(userInfo.Name))
            user.Name = userInfo.Name;

        await userManager.UpdateUserInfo(user);
    }
}