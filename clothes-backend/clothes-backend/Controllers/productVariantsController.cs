using clothes_backend.DTO;
using clothes_backend.Models;
using clothes_backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Controllers
{
    [Route("api/product-variants")]
    [ApiController]
    public class productVariantsController : ControllerBase
    {

        private readonly ProductVariantsRepository _productVariantsRepo;
        public productVariantsController(ProductVariantsRepository productVariantsRepo)
        {
            _productVariantsRepo = productVariantsRepo;
        }

        [HttpPost("add")]
        public async Task<ActionResult<GenericResponse<ProductVariants>>> add([FromForm] productVariantsDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                   .Where(x => x.Value?.Errors.Count > 0)
                   .ToDictionary(
                       error => error.Key,
                       error => error.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                var fullErrorMessage =
                    string.Join(";", errors.Select(error => $"{error.Key}: {string.Join(", ", error.Value)}"));
                return BadRequest(GenericResponse<ProductVariants>.Fail(fullErrorMessage));
            }
            object? result = await _productVariantsRepo.ADD(DTO);
            if (result == null)
            {
                return BadRequest(GenericResponse<ProductVariants>.Fail());
            }
            return Ok(result);
        }
    }
}
