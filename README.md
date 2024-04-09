# Hi this is ZMapper your zimple dto mapper using expression


## Development
```DI
    services.AddScoped<MappingConfiguration>(sp =>
    {
        var config = new MappingConfiguration();

        config.AddConfiguration<User, UserDto>(c =>
        {
            c.CreateMap(src => src.Role.Name, dst => dst.RoleName);
            c.CreateMap(src => src.Email, dst => dst.Email);
        });

        config.AddConfiguration<Role, RoleDto>(c =>
        {
            c.CreateMap(src => src.Id, dst => dst.Id);
            c.CreateMap(src => src.Name, dst => dst.Name);
        });
        return config;
    });
```
``` calling
var db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationDb>();

var config = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MappingConfiguration>();

var users = db.Users.Include(c => c.Role).FirstOrDefault();
var role = db.Set<Role>().FirstOrDefault();

var dto = config.MapTo<Role, RoleDto>(role);
Console.WriteLine(dto);

var dto1 = config.MapTo<User, UserDto>(users);
Console.WriteLine(dto1);

Console.ReadKey();
![image](https://github.com/dadotnetkid/ZMapper/assets/13300183/da8506dc-f867-4bc0-82cd-614e0c9ea18d)

```
