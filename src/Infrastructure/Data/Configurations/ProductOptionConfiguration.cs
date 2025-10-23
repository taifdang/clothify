using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>
{
    public void Configure(EntityTypeBuilder<ProductOption> builder)
    {
        builder.ToTable(nameof(ProductOption));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasIndex(po => new { po.ProductId, po.OptionId }).IsUnique();

        builder.HasOne(po => po.Products)
               .WithMany(p => p.ProductOptions)
               .HasForeignKey(po => po.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(po => po.Options)
               .WithMany(o => o.ProductOptions)
               .HasForeignKey(po => po.OptionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ProductId)
            .HasFilter("[AllowImages] = 1")
            .IsUnique();

    }
}
