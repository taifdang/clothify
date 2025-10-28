using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CartitemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable(nameof(CartItem));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.UnitPrice)
               .HasPrecision(18, 2);

        builder.HasOne(ci => ci.Carts)
               .WithMany(c => c.CartItems)
               .HasForeignKey(ci => ci.CartId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ci => ci.ProductVariants)
               .WithMany(pv => pv.CartDetails)
               .HasForeignKey(ci => ci.ProductVariantId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ci => ci.Version).IsConcurrencyToken();            
    }
}
