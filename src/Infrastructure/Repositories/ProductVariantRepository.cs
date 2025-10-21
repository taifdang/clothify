using Infrastructure.Data;
using Infrastructure.Enitites;
using Infrastructure.Interface;

namespace Infrastructure.Repositories;

public class ProductVariantRepository(ApplicationDbContext context) : BaseRepository<ProductVariant>(context), IProductVariantRepository 
{
}
