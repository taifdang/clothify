using Application.Common.Interface;
using Application.Common.Models.Option;
using Application.Common.Models.OptionValue;
using Application.Common.Models.Product;
using AutoMapper;
using Infrastructure.Enitites;
using Infrastructure.Interface;
using Infrastructure.Models;
using Shared.Models.ProductImage;

namespace Application.Services;

public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ProductDTO> GetById(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(
                filter: x => x.Id == id,
                selector: x => new ProductDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    OldPrice = x.OldPrice,
                    Description = x.Description,
                    Category = x.Categories.Value,
                    ProductType = x.Categories.ProductTypes.Title,
                    Images = x.ProductImages.Select(y => new ProductImageDTO { Id = y.Id, Image = y.Image}).ToList(),
                    Options = x.ProductOptions.Select(y => new OptionDTO
                    {
                        Id = y.OptionId,
                        Title = y.Options.Title,
                        Values = y.Options.OptionValues.Select(z => new OptionValueDTO
                        {
                            Id = z.Id,
                            Value = z.Value                      
                        }).ToList()
                        
                    }).ToList()
                }  
            );
        return product;

    }
    public async Task<Pagination<ProductDTO>> GetList(int pageIndex, int pageSize)
    {
        var products = await _unitOfWork.ProductRepository.ToPagination(
            pageIndex: pageIndex,
            pageSize: pageSize,
            orderBy: x => x.Title,
            ascending: true,
            selector: x => new ProductDTO
            {
                 Id = x.Id,
                 Title = x.Title,
                 Price = x.Price,
                 OldPrice = x.OldPrice,
                 Description = x.Description,
                 Category = x.Categories.Value,
                 ProductType = x.Categories.ProductTypes.Title,
                 Images = x.ProductImages.Select(y => new ProductImageDTO { Id = y.Id, Image = y.Image }).ToList(),
                 Options = x.ProductOptions.Select(y => new OptionDTO
                 {
                     Id = y.OptionId,
                     Title = y.Options.Title,
                     Values = y.Options.OptionValues.Select(z => new OptionValueDTO
                     {
                         Id = z.Id,
                         Value = z.Value
                     }).ToList()
                 }).ToList()
            }
        );

        return products;
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
