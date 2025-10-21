using Application.Common.Interface;
using Application.Common.Models.ProductVariant;
using AutoMapper;
using Infrastructure.Enitites;
using Infrastructure.Interface;

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
}
