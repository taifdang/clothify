using clothes_backend.DTO;
using clothes_backend.Models;
using clothes_backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace clothes_backend.Controllers
{
    [Route("api/product-variants")]
    [ApiController]
    public class productVariantsController : ControllerBase
    {

        private readonly ProductVariantsRepository _productVariantsRepo;
        private readonly DatabaseContext _db;
        public productVariantsController(ProductVariantsRepository productVariantsRepo,DatabaseContext databaseContext)
        {
            _productVariantsRepo = productVariantsRepo;
            _db = databaseContext;
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

            //
            object? result = await _productVariantsRepo.add(DTO);
            if (result == null)
            {
                return BadRequest(GenericResponse<ProductVariants>.Fail((string?)result));
            }
            return Ok(result);
        }
       
    }
}
