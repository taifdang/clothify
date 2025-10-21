using Infrastructure.Data;
using Infrastructure.Enitites;
using Infrastructure.Interface;

namespace Infrastructure.Repositories;

public class CartRepository(ApplicationDbContext context) : BaseRepository<Cart>(context), ICartRepository
{
}
