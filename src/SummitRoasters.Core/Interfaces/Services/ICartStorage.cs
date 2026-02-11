using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Services;

public interface ICartStorage
{
    Cart GetCart();
    void SaveCart(Cart cart);
    void ClearCart();
}
