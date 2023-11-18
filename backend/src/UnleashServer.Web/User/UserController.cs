using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnleashServer.Web.Filters;
using UnleashServer.Web.Mapping;

namespace UnleashServer.Web.User;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int? offset = null)
    {
        return Ok(await _userService.GetUsers(User?.ToInfo(), offset));
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userService.GetCurrent(User?.ToInfo());
        if (user == null)
            return NotFound();
        return Ok(user);
    }


    [HttpPost("{email}/promote")]
    [RequiredAdminRole]
    public async Task<IActionResult> Promote([FromRoute] string email)
    {
        await _userService.Promote(email, User?.ToInfo());
        return Ok();
    }

    [HttpPost("{email}/demote")]
    [RequiredAdminRole]
    public async Task<IActionResult> Demote([FromRoute] string email)
    {
        await _userService.Demote(email, User?.ToInfo());
        return Ok();
    }
}