using System.Text.Json.Serialization;
using Skidbladnir.Repository.Abstractions;

namespace UnleashServer.Users;

public class User : UserInfo, IHasId<string>
{
    public string Id { get; set; }

    public bool Enable { get; set; } = true;

    public UserRole Role { get; set; }
}