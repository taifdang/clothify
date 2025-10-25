using Application.Common.Interface;
using Application.Common.Models.Product;
using AutoMapper;
using Infrastructure.Enitites;
using Infrastructure.Interface;
using Infrastructure.Models;
using Shared.Models.Option;
using Shared.Models.OptionValue;
using Shared.Models.Product;
using Shared.Models.ProductImage;

namespace Application.Services;

public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ProductReadModel> GetById(int id)
    {
        var productReadModel = await _unitOfWork.ProductRepository.GetByIdAsync(
                filter: x => x.Id == id,
                selector: x => new ProductReadModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    OldPrice = x.OldPrice,
                    Description = x.Description,
                    Category = x.Categories.Value,
                    ProductType = x.Categories.ProductTypes.Title,
                    Images = x.ProductImages.Select(img => new ProductImageReadModel
                    {
                        Id = img.Id,
                        Image = img.Image
                    }).ToList(),
                    Options = x.ProductOptions.Select(po => new OptionReadModel
                    {
                        Title = po.Options.Title,
                        Label = po.OptionId,
                        Values = po.Options.OptionValues.Select(v => v.Value).ToList()
                    }).ToList(),
                    OptionValues = x.ProductOptions.Select(po => new OptionValueReadModel
                    {
                        Title = po.Options.Title,
                        OptionId = po.OptionId,
                        Values = po.Options.OptionValues.Select(v => v.Value).ToList(),
                        Options = po.Options.OptionValues.Select(ov => new OptionValueImageReadModel
                        {
                            Title = ov.Value,
                            Label = ov.Label,
                            Image = ov.ProductImages.Select(pi => new ProductImageReadModel
                            {
                                Id = pi.Id,
                                Image = pi.Image
                            }).ToList()
                        }).ToList()
                    }).ToList()
                });
        return productReadModel;
    }
    public async Task<Pagination<ProductReadModel>> GetList(int pageIndex, int pageSize)
    {
        var productsReadModel = await _unitOfWork.ProductRepository.ToPagination(
                pageIndex: pageIndex,
                pageSize: pageSize,
                orderBy: x => x.Title,
                ascending: true,
                selector: x => new ProductReadModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    OldPrice = x.OldPrice,
                    Description = x.Description,
                    Category = x.Categories.Value,
                    ProductType = x.Categories.ProductTypes.Title,
                    Images = x.ProductImages.Select(img => new ProductImageReadModel
                    {
                        Id = img.Id,
                        Image = img.Image
                    }).ToList(),
                    Options = x.ProductOptions.Select(po => new OptionReadModel
                    {
                        Title = po.Options.Title,
                        Label = po.OptionId,
                        Values = po.Options.OptionValues.Select(v => v.Value).ToList()
                    }).ToList(),
                    OptionValues = x.ProductOptions.Select(po => new OptionValueReadModel
                    {
                        Title = po.Options.Title,
                        OptionId = po.OptionId,
                        Values = po.Options.OptionValues.Select(v => v.Value).ToList(),
                        Options = po.Options.OptionValues.Select(ov => new OptionValueImageReadModel
                        {
                            Title = ov.Value,
                            Label = ov.Label,
                            Image = ov.ProductImages.Select(pi => new ProductImageReadModel
                            {
                                Id = pi.Id,
                                Image = pi.Image
                            }).ToList()
                        }).ToList()
                    }).ToList()
                });
        return productsReadModel;
    }

    public async Task<ProductDTO> Add(AddProductRequest request, CancellationToken token)
    {
        var product = _mapper.Map<Product>(request);

        await _unitOfWork.ExecuteTransactionAsync(async() => await _unitOfWork.ProductRepository.AddAsync(product), token);

        return _mapper.Map<ProductDTO>(product);
    }
    
    public async Task<ProductDTO> Update(int id, UpdateProductRequest request, CancellationToken token)
    {
        var product = await _unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == id)
           ?? throw new Exception();

        _mapper.Map(request, product);

        await _unitOfWork.SaveChangesAsync(token);

        return _mapper.Map<ProductDTO>(product);
    }
    public async Task<ProductDTO> Delete(int id, CancellationToken token)
    {
        var product = await _unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception();
        
        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ProductRepository.Delete(product), token);

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> Publish(int id, CancellationToken token)
    {
        var product = await _unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == id) 
            ?? throw new Exception();

        product.Status = "Visible";

        await _unitOfWork.SaveChangesAsync(token);

        return _mapper.Map<ProductDTO>(product);
    }
}
