using clothes_backend.DTO.General;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using Hangfire;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

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
      
        public async Task<PayloadDTO<Products>> DeleteProduct(int id)
        {
            var data = await _productRepository.deleteProduct(id);
            if(data !=null) _backgroundJobClient.Enqueue(() => _backgroundJob.FireAndForgetJob(data));
            return PayloadDTO<Products>.OK(null!);
        }

        public async Task<PayloadDTO<List<productListDTO>?>> GetAllProductAsync()
        {
            var product = await _productRepository.getProductAllAsync();
            return PayloadDTO<List<productListDTO>?>.OK(product);
        }
    }
}
