using clothes_backend.DTO;
using clothes_backend.Models;
using clothes_backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class imageController : ControllerBase
    {
        private readonly ProductOptionImageRepository _imageRepo;
        public imageController(ProductOptionImageRepository imageRepo) 
        {
            _imageRepo = imageRepo;
        }
        [HttpPost("add")]
        public async Task<IActionResult> add([FromForm]imagesDTO DTO)
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

                return BadRequest(GenericResponse<Products>.Fail(fullErrorMessage));
            }
            var result = await _imageRepo.add(DTO);
            if (result == null) return BadRequest(GenericResponse<Products>.Fail());
            return Ok(GenericResponse<Products>.Success(null));
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> delete(int id)
        {
            var result = await _imageRepo.delete(id);
            if (result == null) return BadRequest(GenericResponse<Products>.Fail());
            return Ok(GenericResponse<Products>.Success(null));
        }

    }
}
