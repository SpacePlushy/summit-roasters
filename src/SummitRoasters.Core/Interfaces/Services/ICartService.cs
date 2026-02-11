using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Services;

public interface ICartService
{
    Cart GetCart();
    Task<Cart> AddItemAsync(AddToCartDto dto);
    Cart UpdateItemQuantity(int productId, int quantity);
    Cart RemoveItem(int productId);
    void ClearCart();
    int GetItemCount();
}
