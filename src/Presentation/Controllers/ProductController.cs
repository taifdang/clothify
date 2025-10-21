using Application.Common.Interface;
using Application.Common.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UserController(IProductService productService) : BaseController
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

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Add(AddProductRequest request, CancellationToken token)
      => Ok(await productService.Add(request, token));

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
}
