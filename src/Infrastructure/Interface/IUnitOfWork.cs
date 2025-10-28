namespace Infrastructure.Interface;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IOrderRepository OrderRepository { get; }
    IProductImageRepository ProductImageRepository { get; }
    IProductVariantRepository ProductVariantRepository { get; }
    IProductRepository ProductRepository { get; }
    ICartRepository CartRepository { get; }
    IProductOptionRepository ProductOptionRepository { get; }
    IOptionValueRepository OptionValueRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IForgotPasswordRepository ForgotPasswordRepository { get; }
    Task SaveChangesAsync(CancellationToken token);
    Task ExecuteTransactionAsync(Action action, CancellationToken token);
    Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token);
}
