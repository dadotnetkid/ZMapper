using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DotMapper;

internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDb>
{
    ApplicationDb IDesignTimeDbContextFactory<ApplicationDb>.CreateDbContext(string[] args)
    {


        var builder = new DbContextOptionsBuilder<ApplicationDb>();
        var connectionString =
            "Data Source=localhost;Initial Catalog=TeestDb;Integrated Security=True;Multiple Active Result Sets=True;Encrypt=True;Trust Server Certificate=True";
        builder.UseSqlServer(connectionString);

        return new ApplicationDb(builder.Options);
    }
}