using clothes_backend.DTO.VARIANT;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Repository
{
    public interface IVariantRepository:IBaseRepository<ProductVariants>
    {      
        Task<ProductVariants> AddVariant(ProductVariants DTO,Products products, Dictionary<string, List<variableDTO>> dictionary);
        Task<Products> FindProductsAsync(int id);    
        List<int> GetOptionValueId(int id);
        Dictionary<int,OptionValues> GetProductOptions(int id);
        List<List<int>> GetVariantOption(int Id);
    }
}
