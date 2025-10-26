using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server=LAPTOP-J20BGGNG\\SQLEXPRESS;Database=clothify_db;Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=True");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
