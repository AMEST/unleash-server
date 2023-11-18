using UnleashServer.Users;

namespace UnleashServer.Web.User;

public interface IUserService
{
    Task<IEnumerable<UserModel>> GetUsers(UserInfo requestedBy, int? offset = null);
    Task<UserModel> GetCurrent(UserInfo requestedBy);
    Task<UserModel> GetByEmail(string email, UserInfo requestedBy);
    Task Promote(string email, UserInfo promotedBy);
    Task Demote(string email, UserInfo demotedBy);
}