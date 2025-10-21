using Infrastructure.Enitites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext
{
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ProductType> ProductTypes { get; set; } = default!;
    public DbSet<Category> Categories { get; set; }
    public DbSet<Variant> Variants { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; } = default!;
    public DbSet<Option> Options { get; set; } = default!;
    public DbSet<OptionValue> OptionValues { get; set; } = default!;
    public DbSet<ProductOption> ProductOptions { get; set; } = default!;
    public DbSet<ProductImage> ProductImages { get; set; } = default!;
    public DbSet<Cart> Carts { get; set; } = default!;
    public DbSet<CartDetail> CartDetails { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = default!;
    public DbSet<OrderHistory> OrderHistories { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<RevokeToken> RevokeTokens { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
