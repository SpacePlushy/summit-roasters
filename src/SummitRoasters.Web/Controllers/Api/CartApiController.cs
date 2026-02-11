using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Services;

namespace SummitRoasters.Web.Controllers.Api;

[Route("api/cart")]
[ApiController]
public class CartApiController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartApiController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddToCartDto dto)
    {
        var cart = await _cartService.AddItemAsync(dto);

        return Ok(new
        {
            itemCount = cart.TotalItems,
            subtotal = cart.Subtotal
        });
    }

    [HttpPut("update")]
    public IActionResult Update([FromBody] UpdateCartItemRequest request)
    {
        var cart = _cartService.UpdateItemQuantity(request.ProductId, request.Quantity);

        return Ok(new
        {
            itemCount = cart.TotalItems,
            subtotal = cart.Subtotal
        });
    }

    [HttpDelete("remove/{productId}")]
    public IActionResult Remove(int productId)
    {
        var cart = _cartService.RemoveItem(productId);

        return Ok(new
        {
            itemCount = cart.TotalItems,
            subtotal = cart.Subtotal
        });
    }

    [HttpGet("count")]
    public IActionResult Count()
    {
        var count = _cartService.GetItemCount();

        return Ok(new { count });
    }
}

public class UpdateCartItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
