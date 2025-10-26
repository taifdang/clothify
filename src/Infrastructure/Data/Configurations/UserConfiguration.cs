using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //builder.HasKey(x => x.Id);
        //builder.Property(x => x.Id).ValueGeneratedOnAdd();

        //builder.HasMany(u => u.Carts)
        //       .WithOne(c => c.Users)
        //       .HasForeignKey(c => c.UserId)
        //       .OnDelete(DeleteBehavior.Cascade);

        //builder.HasMany(u => u.Orders)
        //       .WithOne(o => o.Users)
        //       .HasForeignKey(o => o.UserId)
        //       .OnDelete(DeleteBehavior.Cascade);
    }
}

