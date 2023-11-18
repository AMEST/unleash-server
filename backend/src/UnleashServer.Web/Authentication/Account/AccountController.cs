using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnleashServer.Web.Mapping;

namespace UnleashServer.Web.Authentication.Account;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult Login([FromQuery] string redirectUri = null)
    {
        var redirect = "/";
        if (!string.IsNullOrEmpty(redirectUri) && redirectUri.StartsWith("/"))
            redirect = redirectUri;

        var user = User?.ToInfo();
        if (user != null)
            return Redirect(redirect);

        var properties = new AuthenticationProperties()
        {
            RedirectUri = redirect
        };
        return Challenge(properties, "OpenIdConnect");
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
}