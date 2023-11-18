namespace UnleashServer.Web.User;

public class UserModel
{
    public string Email { get; set; }

    public string Name { get; set; }

    public string AvatarUrl { get; set; }

    public UserRoleModel Role { get; set; }
}
