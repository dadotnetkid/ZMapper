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
