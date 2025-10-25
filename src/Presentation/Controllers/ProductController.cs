using Application.Common.Interface;
using Application.Common.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _productService.GetById(id));

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetList(int pageIndex = 0, int pageSize = 10)
        => Ok(await _productService.GetList(pageIndex, pageSize));

    [Authorize]
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Add(AddProductRequest request, CancellationToken token)
        => Ok(await _productService.Add(request, token));

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(int id, UpdateProductRequest request, CancellationToken token)
        => Ok(await _productService.Update(id, request, token));

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
        => Ok(await _productService.Delete(id, token));

    [Authorize]
    [HttpPost("{id}/publish")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Publish(int id, CancellationToken token)
        => Ok(await _productService.Publish(id, token));
}
