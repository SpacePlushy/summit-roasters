using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.Interfaces.Services;

namespace SummitRoasters.Web.Controllers.Api;

[Route("api/products")]
[ApiController]
public class ProductsApiController : ControllerBase
{
    private readonly ISearchService _searchService;

    public ProductsApiController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Ok(Array.Empty<object>());

        var products = await _searchService.AutocompleteAsync(q, 5);

        var results = products.Select(p => new
        {
            id = p.Id,
            name = p.Name,
            slug = p.Slug,
            price = p.Price,
            imageUrl = p.ImageUrl
        });

        return Ok(results);
    }
}
