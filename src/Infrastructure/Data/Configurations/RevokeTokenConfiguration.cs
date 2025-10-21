using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class RevokeTokenConfiguration : IEntityTypeConfiguration<RevokeToken>
{
    public void Configure(EntityTypeBuilder<RevokeToken> builder)
    {
        builder.ToTable(nameof(RevokeToken));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}
