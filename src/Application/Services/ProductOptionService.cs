using Application.Common.Interface;
using AutoMapper;
using Infrastructure.Enitites;
using Infrastructure.Interface;
using Shared.Models.ProductOption;

namespace Application.Services;

public class ProductOptionService(IUnitOfWork unitOfWork, IMapper mapper) : IProductOptionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public Task<List<ProductOptionDTO>> Get(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductOptionDTO> Add(AddProductOptionRequest request, CancellationToken token)
    {
        if (request.AllowImage)
            if (await _unitOfWork.ProductOptionRepository.AnyAsync(x => x.ProductId == request.ProductId && x.AllowImages))
                throw new Exception();

        var productOption = _mapper.Map<ProductOption>(request);

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ProductOptionRepository.AddAsync(productOption), token);

        return _mapper.Map<ProductOptionDTO>(productOption);
    }

    public async Task<ProductOptionDTO> Update(UpdateProductOptionRequest request, CancellationToken token)
    {
        if (request.AllowImage)
            if (await _unitOfWork.ProductOptionRepository.AnyAsync(x => x.Id == x.Id && x.AllowImages))
                throw new Exception();

        var productOption = _mapper.Map<ProductOption>(request);
        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ProductOptionRepository.Update(productOption), token);
        return _mapper.Map<ProductOptionDTO>(productOption);
    }

    public async Task<ProductOptionDTO> Delete(int Id, CancellationToken token)
    {
        var existProductOption = await _unitOfWork.ProductOptionRepository.FirstOrDefaultAsync(x => x.Id == Id)
            ?? throw new Exception();

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ProductOptionRepository.Delete(existProductOption), token);
        return _mapper.Map<ProductOptionDTO>(existProductOption);
    }
}
