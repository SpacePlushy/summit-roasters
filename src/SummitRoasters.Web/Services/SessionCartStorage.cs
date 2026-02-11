using System.Text.Json;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Web.Services;

public class SessionCartStorage : ICartStorage
{
    private const string CartSessionKey = "ShoppingCart";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionCartStorage(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Cart GetCart()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return new Cart();

        var json = session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(json)) return new Cart();

        return JsonSerializer.Deserialize<Cart>(json) ?? new Cart();
    }

    public void SaveCart(Cart cart)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return;

        var json = JsonSerializer.Serialize(cart);
        session.SetString(CartSessionKey, json);
    }

    public void ClearCart()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.Remove(CartSessionKey);
    }
}
