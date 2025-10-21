using Infrastructure.Data;
using Infrastructure.Enitites;
using Infrastructure.Interface;

namespace Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
{

}
