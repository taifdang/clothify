using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable(nameof(ProductImage));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(poi => poi.OptionValues)
               .WithMany(ov => ov.ProductImages)
               .HasForeignKey(poi => poi.OptionValueId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(poi => poi.Products)
               .WithMany(p => p.ProductImages)
               .HasForeignKey(poi => poi.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
