using Application.Common.Interface;
using Application.Common.Models.ProductVariant;
using Application.Common.Utilities;
using AutoMapper;
using Infrastructure.Enitites;
using Infrastructure.Interface;
using Shared.Models.OptionValue;
using Shared.Models.ProductVariant;

namespace Application.Services;

public class ProductVariantService(
    IUnitOfWork unitOfWork,
    IProductVariantFilterService filterService, 
    IMapper mapper) : IProductVariantService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IProductVariantFilterService _filterService = filterService;
    public async Task<List<ProductVariantReadModel>> GetList(int productId, Dictionary<string, string>? selectedOptions)
    {
        _filterService.FilterByProductId(productId);
        
        if(selectedOptions != null)
        {
            _filterService.FilterByOptionValues(selectedOptions);
        }

        var _filter = _filterService.BuildCombinedFilter();

        var productVariants = await _unitOfWork.ProductVariantRepository.GetListAsync(
                filter: _filter,
                selector: x => new ProductVariantReadModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    OldPrice = x.OldPrice,
                    Percent = x.Percent,
                    Quantity = x.Quantity,
                    Sku = x.Sku,
                    Options = x.Variants.Select(y => new OptionValueVariantReadModel
                    {
                        Title = y.OptionValues.OptionId,
                        Value = y.OptionValues.Value
                    }).ToList()
                });

        return productVariants;
        #region
        //    filter: x => x.ProductId == id && x.Variants.All(x => optionIds.Contains(x.OptionValueId)),      
        //    selector: x => new ProductVariantDTO
        #endregion
    }

    public async Task<ProductVariantReadModel> GetById(int productId, int id)
    {
        var productVariant = await _unitOfWork.ProductVariantRepository.GetByIdAsync(
                filter: x => x.ProductId == productId && x.Id == id,
                selector: x => new ProductVariantReadModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    OldPrice = x.OldPrice,
                    Percent = x.Percent,
                    Quantity = x.Quantity,
                    Sku = x.Sku,
                    Options = x.Variants.Select(y => new OptionValueVariantReadModel
                    {
                        Title = y.OptionValues.OptionId,
                        Value = y.OptionValues.Value
                    }).ToList()
                });

        return productVariant;
    }

    public async Task<ProductVariantDTO> Add(int productId, AddProductVariantRequest request, CancellationToken token)
    {
        var optionValueMap = await _unitOfWork.OptionValueRepository.GetListAsync(
               filter: x => x.Options.ProductOptions.Any(y => y.ProductId == productId),
               selector: x => new{ x.Id, x.OptionId });

        if (!optionValueMap.Any())
            throw new Exception();

        var validOptionValueId = optionValueMap.Select(x => x.Id).ToHashSet();

        if (request.OptionValueIds.Any(id => !validOptionValueId.Contains(id)))
            throw new Exception();

        // check duplicate option value
        var selectedOptionIds = optionValueMap
            .Where(v => request.OptionValueIds.Contains(v.Id))
            .Select(v => v.OptionId)
            .ToList();

        if (selectedOptionIds.Count != selectedOptionIds.Distinct().Count())
            throw new Exception();

        var totalOptionCount = optionValueMap.Select(v => v.OptionId).Distinct().Count();
        if (selectedOptionIds.Count != totalOptionCount)
            throw new Exception();

        _filterService.FilterByProductId(productId);
        _filterService.FilterByExactOptionValues(request.OptionValueIds);
        _filterService.FilterByOptionCount(request.OptionValueIds);

        var _filter = _filterService.BuildCombinedFilter();

        if (await _unitOfWork.ProductVariantRepository.AnyAsync(filter: _filter))
            throw new Exception();

        var variant = new ProductVariant
        {
            ProductId = productId,
            Price = request.Price,
            OldPrice = request.OldPrice,
            Quantity = request.Quantity,
            Variants = request.OptionValueIds.Select(id => new Variant
            {
                OptionValueId = id
            }).ToList()
        };

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ProductVariantRepository.AddAsync(variant), token);

        return _mapper.Map<ProductVariantDTO>(variant);
    }

    public async Task<ProductVariantDTO> Update(int productId, int id, UpdateProductVariantRequest request, CancellationToken token)
    {
        var productVariant = await _unitOfWork.ProductVariantRepository.FirstOrDefaultAsync(x => x.ProductId == productId && x.Id == id) 
            ?? throw new Exception();

        _mapper.Map(request, productVariant);

        await _unitOfWork.SaveChangesAsync(token);

        return _mapper.Map<ProductVariantDTO>(productVariant);
    }

    public async Task<ProductVariantDTO> Delete(int productId, int id, CancellationToken token)
    {
        var productVariant = await _unitOfWork.ProductVariantRepository.FirstOrDefaultAsync(x => x.Id == id && x.ProductId == productId)
           ?? throw new Exception();

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ProductVariantRepository.Delete(productVariant), token);

        return _mapper.Map<ProductVariantDTO>(productVariant);
    }

    public async Task<int> Generate(int id, GenerateVariantsRequest optionValues, CancellationToken token)
    {
        if (optionValues == null || optionValues.OptionValues.Count == 0)
            throw new ArgumentException("No option values provided.");

        // get optionValue
        var listoptionValueId = optionValues.OptionValues.SelectMany(x => x.Value).ToList();

        var optionValueEntity = await _unitOfWork.OptionValueRepository.GetListAsync(
            filter: x => listoptionValueId.Contains(x.Id),
            selector: x => new { x.Id, x.Label, x.Value});

        var optionDictation = optionValueEntity.ToDictionary(x => x.Id, x => x.Label ?? x.Value);

        // generate
        var listOptionValue = optionValues.OptionValues.Values.ToList();
        // var combinations = CartesianHelper.CartesianProduct(listOptionValue);

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(
            filter: x => x.Id == id,
            selector: x => new
            {
                x.Id,
                x.Price,
                x.OldPrice,
                Sku = string.Join(" - ", x.Categories.Label, x.Id)
            });

        if (product == null)
            throw new Exception("Product not found");

        const int batchSize = 100;
        var batch = new List<ProductVariant>(batchSize);

        foreach (var combination in CartesianHelper.CartesianProduct(listOptionValue))
        {
            var optionIds = combination.ToList();

            var title = string.Join(" - ", optionIds.Select(id => optionDictation[id]));

            var variant = new ProductVariant
            {
                ProductId = product.Id,
                Title = title,
                Price = product.Price,
                OldPrice = product.OldPrice,
                Quantity = 0,
                Percent = 0,
                Sku = $"{product.Sku}-{string.Join("-", optionIds)}",
                Variants = optionIds.Select(x => new Variant
                {
                    OptionValueId = x
                }).ToList()
            };

            batch.Add(variant);

            if (batch.Count >= batchSize)
            {
                await _unitOfWork.ProductVariantRepository.AddRangeAsync(batch);
                await _unitOfWork.SaveChangesAsync(token);
                batch.Clear();
            }
        }

        if (batch.Count > 0)
        {
            await _unitOfWork.ProductVariantRepository.AddRangeAsync(batch);
            await _unitOfWork.SaveChangesAsync(token);
        }

        return id;
    }

}
