using Application.Common.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.ProductImage;

namespace Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductImageController(IProductImageService productImageService) : ControllerBase
{
    private readonly IProductImageService _productImageService = productImageService;

    #region
    /*
    [HttpGet("{productId}/images")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetList(int productId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{productId}/images/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int productId, int id)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{productId}/images/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int productId, int id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{productId}/images/option-value/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProductImageByOptionValueId(int productId, int id)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{fileName}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult GetFileUrl(string fileName)
        => throw new NotImplementedException();
    */
    #endregion

    [HttpPost("{productId}/images")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Add(int productId, [FromForm] AddProductImageRequest request, CancellationToken token)
        => Ok(await _productImageService.Add(productId, request, token));

    [HttpDelete("{productId}/images/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int productId, int id, CancellationToken token)
        => Ok(await _productImageService.Delete(productId, id, token));
}
