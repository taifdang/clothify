using Application.Common.Interface;
using AutoMapper;
using Infrastructure.Enitites;
using Infrastructure.Interface;
using Shared.Models.Media;
using Shared.Models.ProductImage;

namespace Application.Services;

public class ProductImageService(IUnitOfWork unitOfWork, IMapper mapper, IFileService storageService) : IProductImageService
{
    public IUnitOfWork _unitOfWork = unitOfWork;
    public IMapper _mapper = mapper;
    private IFileService _storageService = storageService;

    public async Task<ProductImageDTO> Add(AddProductImageRequest request, CancellationToken token)
    {
        if (request.OptionValueId.HasValue)
        {
            if (await _unitOfWork.OptionValueRepository.AnyAsync(x => x.Id == request.OptionValueId.Value && x.Options.ProductOptions.Any(y => y.ProductId == request.ProductId && y.AllowImages)))
                throw new Exception();
        }

        // Save file
        if(request.MediaFile != null)
        {
            var pathMedia = await _storageService.AddFileAsync(request.MediaFile);

            //var productImage = _mapper.Map<ProductImage>(request);
            //productImage.Image = pathMedia.Path;

            var productImage = new ProductImage
            {
                ProductId = request.ProductId,
                OptionValueId = request.OptionValueId.Value,
                Image = pathMedia.Path
            };

            await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.ProductImageRepository.AddAsync(productImage), token);
        }
    
        return _mapper.Map<ProductImageDTO>(request);
    }

    public async Task<ProductImageDTO> Delete(int id, CancellationToken token)
    {
        var productImage = await _unitOfWork.ProductImageRepository.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception();

        // Remove file
        if (productImage != null)
        {
            if(productImage.Image != null)
                await _storageService.DeleteFileAsync(new DeleteFileRequest { FileName = productImage.Image });
            await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ProductImageRepository.Delete(productImage), token);
        }

        return _mapper.Map<ProductImageDTO>(productImage);
    }
}
