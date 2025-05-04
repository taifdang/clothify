using clothes_backend.DTO.General;
using clothes_backend.DTO.IMAGE;
using clothes_backend.Interfaces.Service;
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
        private readonly IImageService _service;
        public imageController(ProductOptionImageRepository imageRepo,IImageService service) 
        {
            _imageRepo = imageRepo;
            _service = service;
        }
        [HttpPost("add")]
        public async Task<IActionResult> add([FromForm]imageUploadDTO DTO)
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

                return BadRequest(Result<ProductOptionImages>.IsValid(fullErrorMessage));
            }
            //var result = await _imageRepo.add(DTO);
            //if (result == null) return BadRequest(GenericResponse<Products>.Fail());
            //return Ok(GenericResponse<Products>.Success(null));
            var result = await _service.AddImage(DTO);
            if (result.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(result);
            return Ok(result);
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
