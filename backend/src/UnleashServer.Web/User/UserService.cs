using Microsoft.Extensions.Logging;
using UnleashServer.Users;
using UnleashServer.Web.Mapping;

namespace UnleashServer.Web.User;

internal class UserService : IUserService
{
    private readonly IUserManager _userManager;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserManager userManager,
        ILogger<UserService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IEnumerable<UserModel>> GetUsers(UserInfo requestedBy, int? offset = null)
    {
        var users = await _userManager.GetUsers(offset);
        return users.Select(x => x.ToModel());
    }

    public async Task<UserModel> GetByEmail(string email, UserInfo requestedBy)
    {
        var user = await _userManager.GetByEmail(email);
        return user.ToModel();
    }

    public async Task<UserModel> GetCurrent(UserInfo requestedBy)
    {
        return await GetByEmail(requestedBy.Email, requestedBy);
    }

    public async Task Promote(string email, UserInfo promotedBy)
    {
        var user = await _userManager.GetByEmail(email);
        await _userManager.ChangeRole(user, UserRole.Admin);
        _logger.LogInformation("User `{User}` promote by `{RequestedBy}`", user.Email, promotedBy.Email);
    }

    public async Task Demote(string email, UserInfo demotedBy)
    {
        var user = await _userManager.GetByEmail(email);
        await _userManager.ChangeRole(user, UserRole.User);
        _logger.LogInformation("User `{User}` demote by `{RequestedBy}`", user.Email, demotedBy.Email);
    }

}