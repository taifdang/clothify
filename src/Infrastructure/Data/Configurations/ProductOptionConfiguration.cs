using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>
{
    public void Configure(EntityTypeBuilder<ProductOption> builder)
    {
        builder.ToTable(nameof(ProductOption));

        builder.HasKey(po => new { po.ProductId, po.OptionId });

        builder.HasOne(po => po.Products)
               .WithMany(p => p.ProductOptions)
               .HasForeignKey(po => po.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(po => po.Options)
               .WithMany(o => o.ProductOptions)
               .HasForeignKey(po => po.OptionId)
               .OnDelete(DeleteBehavior.Cascade); throw new NotImplementedException();
    }
}
