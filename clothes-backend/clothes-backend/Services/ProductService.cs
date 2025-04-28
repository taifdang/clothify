using clothes_backend.DTO.General;
using clothes_backend.DTO.PRODUCT;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using clothes_backend.Utils.Enum;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
namespace clothes_backend.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBackgroundJobService _backgroundJob;
        private readonly IBackgroundJobClient _backgroundJobClient;    
        public ProductService(
            IProductRepository productRepository,
            IBackgroundJobService backgroundJob, 
            IBackgroundJobClient backgroundJobClient
            )
        {
            _productRepository = productRepository;
            _backgroundJob = backgroundJob;
            _backgroundJobClient = backgroundJobClient;
           
        }
        public async Task<Result<List<productListDTO>?>> GetAllProductAsync()
        {
            var product = await _productRepository.GetProductAllAsync();
            return Result<List<productListDTO>?>.Success(product);
        }
        public async Task<Result<Products>> DeleteAsync(int id)
        {
            var data = await _productRepository.DeleteAsync(id);
            if (data != null) Result<Products>.Failure(StatusCode.NotFound);
            _backgroundJobClient.Enqueue(() => _backgroundJob.FireAndForgetJob(data));
            return Result<Products>.Success(null!);
        }
        public async Task<Result<Products>> UpdateAsync(int id, [FromForm] ProductDTO DTO)
        {
            var data = await _productRepository.UpdateAsync(id, DTO);
            if (data.statusCode != StatusCode.Success) return Result<Products>.Failure(data.statusCode);
            return Result<Products>.Success();
        }
        public async Task<Result<Products>> AddAsync([FromForm] ProductDTO DTO)
        {
            var data = await _productRepository.AddAsync(DTO);
            if (data == null) return Result<Products>.Failure(StatusCode.Isvalid);
            return Result<Products>.Success(data);
        }

        public async Task<Result<productListDTO>> GetId(int id)
        {
            var data = await _productRepository.GetId(id);
            if (data.statusCode != StatusCode.Success) return Result<productListDTO>.Failure(data.statusCode);
            return Result<productListDTO>.Success(data.data);
        }
    }
}
