using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/options")]
public class OptionValueController
{
    [HttpGet("{optionId}/values")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetListOptionValue(int optionId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{optionId}/values/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOptionValueById(int optionId, int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{optionId}/values")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddOptionValue(int id)
    {
        throw new NotImplementedException();
    }
    [HttpPut("{optionId}/values/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateOptionValue(int id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{optionId}/values/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteOptionValue(int id)
    {
        throw new NotImplementedException();
    }
}
