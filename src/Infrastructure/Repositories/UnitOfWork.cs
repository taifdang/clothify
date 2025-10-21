using Infrastructure.Data;
using Infrastructure.Interface;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IUserRepository UserRepository { get; }
    public IOrderRepository OrderRepository { get; }
    public IProductImageRepository ProductImageRepository { get; }
    public IProductVariantRepository ProductVariantRepository { get; }
    public IProductRepository ProductRepository { get; }
    public ICartRepository CartRepository { get; }
    public IProductOptionRepository ProductOptionRepository { get; }
    public IOptionValueRepository OptionValueRepository { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        UserRepository = new UserRepository(_context);
        OrderRepository = new OrderRepository(_context);
        ProductImageRepository = new ProductImageRepository(_context);
        ProductVariantRepository = new ProductVariantRepository(_context);
        ProductRepository = new ProductRepository(_context);
        CartRepository = new CartRepository(_context);
        ProductOptionRepository = new ProductOptionRepository(_context);
        OptionValueRepository = new OptionValueRepository(_context);
    }

    public async Task ExecuteTransactionAsync(Action action, CancellationToken token)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            action();
            await _context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }
        catch
        {
            await transaction.RollbackAsync(token);
        }
    }

    public async Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            await action();
            await _context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }
        catch
        {
            await transaction.RollbackAsync(token);
        }
    }

    public async Task SaveChangesAsync(CancellationToken token) => await _context.SaveChangesAsync(token);
}
