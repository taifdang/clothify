using Infrastructure.Data;
using Infrastructure.Enitites;
using Infrastructure.Interface;

namespace Infrastructure.Repositories;

public class ProductImageRepository(ApplicationDbContext context) : BaseRepository<ProductImage>(context), IProductImageRepository 
{
}
