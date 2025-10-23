using Application.Common.Interface;
using Application.Common.Models.ProductVariant;
using Application.Common.Utilities;
using AutoMapper;
using Infrastructure.Enitites;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Shared.Models.ProductVariant;
using System.Xml.Schema;

namespace Application.Services;

public class ProductVariantService(IUnitOfWork unitOfWork,IProductVariantFilterService filterService, IMapper mapper) : IProductVariantService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IProductVariantFilterService _filterService = filterService;
    public async Task<List<ProductVariantDTO>> Get(int id, Dictionary<string, string>? selectedOptions)
    {
        _filterService.FilterByOptionValues(selectedOptions);

        var _filter = _filterService.BuildCombinedFilter();

        var productVariants = await _unitOfWork.ProductVariantRepository.GetListAsync(
                filter: _filter,
                selector: x => new ProductVariantDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    OldPrice = x.OldPrice,
                    Percent = x.Percent,
                    Quantity = x.Quantity,
                    Sku = x.Sku,
                    Options = x.Variants.Select(y => y.OptionValues.Value).ToList()
                }
            );

        return productVariants;
        #region
        //var productVariants = await _unitOfWork.ProductVariantRepository.GetListAsync(
        //    filter: x => x.ProductId == id && x.Variants.All(x => optionIds.Contains(x.OptionValueId)),      
        //    selector: x => new ProductVariantDTO
        //    {
        //        Id = x.Id,
        //        Title = x.Title,
        //        Price = x.Price,
        //        OldPrice = x.OldPrice,
        //        Percent = x.Percent,
        //        Quantity = x.Quantity,
        //        Sku = x.Sku,
        //        Options = x.Variants.Select(y => y.OptionValues.Value).ToList()
        //    });
        #endregion
    }
    public async Task<ProductVariantDTO> Add(AddProductVariantRequest request, CancellationToken token)
    {
        var productOptionValueMap = await _unitOfWork.OptionValueRepository.GetListAsync(
               filter: x => x.Options.ProductOptions.Any(x => x.ProductId == request.ProductId),
               selector: x => new{ x.Id, x.OptionId });

        if (!productOptionValueMap.Any())
            throw new Exception();

        var validOptionValueId = productOptionValueMap.Select(x => x.Id).ToHashSet();

        if (request.OptionValueIds.Any(id => !validOptionValueId.Contains(id)))
            throw new Exception();

        // check duplicate option value
        var selectedOptionIds = productOptionValueMap
            .Where(v => request.OptionValueIds.Contains(v.Id))
            .Select(v => v.OptionId)
            .ToList();

        if (selectedOptionIds.Count != selectedOptionIds.Distinct().Count())
            throw new Exception();

        var totalOptionCount = productOptionValueMap.Select(v => v.OptionId).Distinct().Count();
        if (selectedOptionIds.Count != totalOptionCount)
            throw new Exception();

        _filterService.FilterByProductId(request.ProductId);
        _filterService.FilterByExactOptionValues(request.OptionValueIds);
        _filterService.FilterByOptionCount(request.OptionValueIds);

        var _filter = _filterService.BuildCombinedFilter();

        if (await _unitOfWork.ProductVariantRepository.AnyAsync(filter: _filter))
            throw new Exception();

        var variant = new ProductVariant
        {
            ProductId = request.ProductId,
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

    public async Task<ProductVariantDTO> Delete(int id, CancellationToken token)
    {
        var existProductVariant = await _unitOfWork.ProductVariantRepository.FirstOrDefaultAsync(x => x.Id == id)
           ?? throw new Exception();

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ProductVariantRepository.Delete(existProductVariant), token);
        return _mapper.Map<ProductVariantDTO>(existProductVariant);
    }

    public async Task Generate(int id, GenerateVariantsRequest optionValues, CancellationToken token)
    {
        if (optionValues == null || optionValues.OptionValues.Count == 0)
            throw new ArgumentException("No option values provided.");

        // get optionValue
        var listoptionValueId = optionValues.OptionValues.SelectMany(x => x.Value).ToList();

        var optionValueEntity = await _unitOfWork.OptionValueRepository.GetListAsync(
            filter: x => listoptionValueId.Contains(x.Id),
            selector: x => new { x.Id, x.Label, x.Value});

        var optionDictation = optionValueEntity.ToDictionary(x => x.Id, x => x.Label ?? x.Value);

        //generate
        var listOptionValue = optionValues.OptionValues.Values.ToList();
        var combinations = CartesianHelper.CartesianProduct(listOptionValue);

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

        var variants = new List<ProductVariant>();

        foreach (var combination in combinations)
        {
            var optionValueId = combination.ToList();

            var title = string.Join(" - ", optionValueId.Select(x => optionDictation[x]));

            var variant = new ProductVariant
            {
                ProductId = product.Id,
                Title = title,
                Price = product.Price,
                OldPrice = product.OldPrice,
                Quantity = 0,
                Percent = 0,
                Sku = $"{product.Sku}-{string.Join("-", optionValueId)}",
                Variants = optionValueId.Select(x => new Variant
                {
                    OptionValueId = x
                }).ToList()
            };

            variants.Add(variant);
        }

        const int batchSize = 100;

        await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            foreach (var batch in variants.Chunk(batchSize))
            {
                await _unitOfWork.ProductVariantRepository.AddRangeAsync(batch);
            }
        }, token);
    }
}
