using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
{
    public void Configure(EntityTypeBuilder<CartDetail> builder)
    {
        builder.ToTable(nameof(CartDetail));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(ci => ci.Carts)
               .WithMany(c => c.CartDetails)
               .HasForeignKey(ci => ci.CartId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ci => ci.ProductVariants)
               .WithMany(pv => pv.CartDetails)
               .HasForeignKey(ci => ci.ProductVariantId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ci => ci.RowVersion)
               .IsRowVersion();
    }
}
