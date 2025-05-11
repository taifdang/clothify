using AutoMapper;
using clothes_backend.DTO.General;
using clothes_backend.DTO.VARIANT;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;
namespace clothes_backend.Services
{
    public class VariantService : IVariantService
    {
        private readonly IVariantRepository _repository;
        private readonly IMapper _mapper;
        public VariantService(IVariantRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Result<ProductVariants>> AddVariant([FromForm] ProductVariantDTO DTO)
        {
            try
            {
                var Product = await _repository.FindProductsAsync(DTO.product_id);
                if (Product is null) return Result<ProductVariants>.Failure(Utils.Enum.StatusCode.NotFound);
                //lay option_value_id hinh anh cua product
                 var ImageKey = _repository.GetOptionValueId(Product.id);
                if (!DTO.options.Any(x => ImageKey.Contains(x))) return Result<ProductVariants>.Failure(Utils.Enum.StatusCode.NotFound);
                var optionValue = _repository.GetProductOptions(Product.id);
                //
                var dictionary = new Dictionary<string, List<variableDTO>>();
                foreach (var item in DTO.options)
                {
                    var IsOptionValue = optionValue.TryGetValue(item, out var options);
                    if (!IsOptionValue) return Result<ProductVariants>.Failure(Utils.Enum.StatusCode.NotFound);
                    dictionary.Add(options.option_id, new List<variableDTO>
                {
                      new variableDTO { id = options.id, label = options.label }
                });
                }
                //kiem tra voi variant trung lap
                var Variants = _repository.GetVariantOption(Product.id);
                var IsMatch = Variants.Any(x => x.Intersect(DTO.options).Count() == DTO.options.Count());
                if (IsMatch) return Result<ProductVariants>.Failure(Utils.Enum.StatusCode.Isvalid);
                //luu
                var product_variant = _mapper.Map<ProductVariants>(DTO);
                var data = await _repository.AddVariant(product_variant, Product, dictionary);
                if (data is null) return Result<ProductVariants>.Failure(Utils.Enum.StatusCode.Isvalid);
                return Result<ProductVariants>.Success();
            }
            catch
            {
                return Result<ProductVariants>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }

        public async Task<Result<ProductVariants>> DeleteVariant(int id)
        {
           var variant =  await _repository.FindBase(x => x.id == id);
            if (variant is null) return Result<ProductVariants>.Failure(Utils.Enum.StatusCode.NotFound);
            try
            {
                await _repository.DeleteBase(variant);
                return Result<ProductVariants>.Success();
            }
            catch
            {
                return Result<ProductVariants>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }

        public Task<Result<ProductVariants>> GetIdVariant([FromForm] ProductVariantDTO DTO)
        {
            throw new NotImplementedException();
        }
    }
}
