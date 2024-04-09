using System.Text.Json;

namespace DotMapper;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? ClaimsId { get; set; }
    public Claims Claims { get; set; }
    public ICollection<Permission> Permissions { get; set; } = new HashSet<Permission>();
}

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class PermissionDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
