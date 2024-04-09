using DotMapper;
using System.Text.Json;

namespace DotMapper;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public string Claim { get; set; }
    public ICollection<Permission> Permissions { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}