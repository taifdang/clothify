using Application.Common.Interface;
using Application.Common.Models.ProductVariant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.ProductVariant;

namespace Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductVariantController(IProductVariantService productVariantService) : ControllerBase
{
    private readonly IProductVariantService _productVariantService = productVariantService;

    [HttpGet("{productId}/variants")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetList(int productId, Dictionary<string, string>? seletedOptions)
        => Ok(await _productVariantService.GetList(productId, seletedOptions));

    [HttpGet("{productId}/variants/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int productId, int id)
        => Ok(await _productVariantService.GetById(productId, id));

    [Authorize]
    [HttpPost("{productId}/variants")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Add(int productId, AddProductVariantRequest request, CancellationToken token)
        => Ok(await _productVariantService.Add(productId, request, token));

    [Authorize]
    [HttpPut("{productId}/variants/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int productId, int id, UpdateProductVariantRequest request, CancellationToken token)
        => Ok(await _productVariantService.Update(productId, id, request, token));

    [Authorize]
    [HttpDelete("{productId}/variants/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int productId, int id, CancellationToken token)
        => Ok(await _productVariantService.Delete(productId, id, token));

    [Authorize]
    [HttpPost("{productId}/variants/generate")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Generate(int productId, GenerateVariantsRequest request, CancellationToken token)
        => Ok(await _productVariantService.Generate(productId, request, token));
}
