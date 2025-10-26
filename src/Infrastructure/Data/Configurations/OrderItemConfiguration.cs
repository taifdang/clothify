
using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(nameof(OrderItem));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.Price)
               .HasPrecision(18, 2);

        builder.HasOne(od => od.Orders)
               .WithMany(o => o.OrderItems)
               .HasForeignKey(od => od.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(od => od.ProductVariants)
               .WithMany(pv => pv.OrderDetails)
               .HasForeignKey(od => od.ProductVariantId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
