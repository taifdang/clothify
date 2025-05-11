using clothes_backend.Data;
using clothes_backend.DTO.General;
using clothes_backend.DTO.VARIANT;
using clothes_backend.Interfaces.Service;
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
        private readonly IVariantService _service;
        public variantController(IVariantService service)
        {     
            _service = service;
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
                return BadRequest(Result<ProductVariants>.IsValid(fullErrorMessage));
            }         
            var result = await _service.AddVariant(DTO);
            if (result.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            var result = await _service.DeleteVariant(id);
            if (result.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
