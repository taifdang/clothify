using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class ForgotPasswordConfiguration : IEntityTypeConfiguration<ForgotPassword>
{
    public void Configure(EntityTypeBuilder<ForgotPassword> builder)
    {
        builder.ToTable(nameof(ForgotPassword));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}
