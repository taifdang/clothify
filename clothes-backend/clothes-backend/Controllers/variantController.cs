using clothes_backend.Data;
using clothes_backend.DTO.VARIANT;
using clothes_backend.Models;
using clothes_backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class variantController : ControllerBase
    {

        private readonly ProductVariantsRepository _productVariantsRepo;
        private readonly DatabaseContext _db;
        public variantController(ProductVariantsRepository productVariantsRepo,DatabaseContext databaseContext)
        {
            _productVariantsRepo = productVariantsRepo;
            _db = databaseContext;
        }
        [HttpGet("getId")]
        public async Task<IActionResult> getId(int id)
        {
            var result = await _productVariantsRepo.getId(id);
            if(result is null) return BadRequest(GenericResponse<ProductVariants>.Fail());
            return Ok(GenericResponse<ProductVariants>.Success(result));
        }
        [HttpPost("add")]
        public async Task<ActionResult<GenericResponse<ProductVariants>>> add([FromForm] ProductVariantDTO DTO)
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

            //
            object? result = await _productVariantsRepo.add(DTO);
            if (result == null)
            {
                return BadRequest(GenericResponse<ProductVariants>.Fail((string?)result));
            }
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            var result = await _productVariantsRepo.delete(id);
            if(result == 400)  return BadRequest(GenericResponse<ProductVariants>.Fail());
            return Ok(GenericResponse<ProductVariants>.Success(null));
        }
    }
}
