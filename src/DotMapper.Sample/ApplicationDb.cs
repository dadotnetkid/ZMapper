using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotMapper;

public class ApplicationDb : DbContext
{
    public ApplicationDb(DbContextOptions<ApplicationDb> context)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionstring =
            "Data Source=localhost;Initial Catalog=TeestDb;Integrated Security=True;Multiple Active Result Sets=True;Encrypt=True;Trust Server Certificate=True";

        optionsBuilder.UseSqlServer(connectionstring);
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    public DbSet<User> Users { get; set; }
}

/*
 *
 *
 *Expression<Func<User, UserDto>> userMapping = user => new UserDto
   {
       Id = user.Id,
       Name = user.Name,
       Email = user.Email
   };
   
   return users.Select(userMapping);
 */