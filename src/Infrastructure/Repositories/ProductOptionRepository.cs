using Infrastructure.Data;
using Infrastructure.Enitites;
using Infrastructure.Interface;

namespace Infrastructure.Repositories;

public class ProductOptionRepository(ApplicationDbContext context) : BaseRepository<ProductOption>(context), IProductOptionRepository
{
}
