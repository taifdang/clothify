using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable(nameof(ProductVariant));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.RegularPrice)
               .HasPrecision(18, 2);

        builder.Property(p => p.ComparePrice)
               .HasPrecision(18, 2);

        builder.Property(p => p.Percent)
              .HasPrecision(5, 2);
    }
}
