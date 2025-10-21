using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class VariantConfiguration : IEntityTypeConfiguration<Variant>
{
    public void Configure(EntityTypeBuilder<Variant> builder)
    {
        builder.ToTable(nameof(Variant));

        builder.HasKey(v => new { v.ProductVariantId, v.OptionValueId });

        builder.HasOne(v => v.ProductVariants)
               .WithMany(pv => pv.Variants)
               .HasForeignKey(v => v.ProductVariantId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.OptionValues)
               .WithMany(ov => ov.Variants)
               .HasForeignKey(v => v.OptionValueId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
