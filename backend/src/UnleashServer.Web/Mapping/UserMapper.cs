using System.Security.Claims;
using Riok.Mapperly.Abstractions;
using UnleashServer.Users;
using UnleashServer.Web.User;

namespace UnleashServer.Web.Mapping;

[Mapper]
public static partial class UserMapper
{
    public static partial UserModel ToModel(this UserInfo user);

    public static UserInfo ToInfo(this ClaimsPrincipal principal)
    {
        var user = new UserInfo
        {
            Email = principal.FindFirst(ClaimTypes.Email)?.Value?.ToLower() ??
                    principal.FindFirst("email")?.Value?.ToLower(),
            Name = principal.Identity?.Name,
            AvatarUrl = principal.FindFirst("picture")?.Value
        };
        if (string.IsNullOrEmpty(user.Email))
            return null;
        return user;
    }
}