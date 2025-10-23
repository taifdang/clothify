using Application.Common.Interface;
using Application.Common.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.ProductImage;
using Shared.Models.ProductVariant;

namespace Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(IProductService productService, IProductImageService productImageService) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(int id)
        => Ok(await productService.Get(id));

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10)
        => Ok(await productService.Get(pageIndex, pageSize));

    [Authorize]
    [HttpPut]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(UpdateProductRequest request, CancellationToken token)
        => Ok(await productService.Update(request, token));

    [Authorize]
    [HttpDelete]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
        => Ok(await productService.Delete(id, token));

    [Authorize]
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Add(AddProductRequest request, CancellationToken token)
       => Ok(await productService.Add(request, token));

    [Authorize]
    [HttpPost("{id}/options")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddProductOptions(AddProductRequest request, CancellationToken token)
      => Ok(await productService.Add(request, token));
    [Authorize]
    [HttpPost("{id}/option-values")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddOptionValues(AddProductRequest request, CancellationToken token)
     => Ok(await productService.Add(request, token));

    [Authorize]
    [HttpPost("{id}/variants")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddVariants(AddProductRequest request, CancellationToken token)
      => Ok(await productService.Add(request, token));

    [Authorize]
    [HttpPost("{id}/generate-variants")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GenerateVariants(int id, GenerateVariantsRequest request, CancellationToken token)
      => Ok();

    [Authorize]
    [HttpPost("{id}/images")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddImages([FromForm]AddProductImageRequest request, CancellationToken token)
     => Ok(await productImageService.Add(request, token));
    [Authorize]
    [HttpPost("{id}/publish")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ApplyImport(AddProductRequest request, CancellationToken token)
     => Ok(await productService.Add(request, token));
}
