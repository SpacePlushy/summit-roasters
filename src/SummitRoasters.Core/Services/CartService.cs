using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Services;

public class CartService : ICartService
{
    private readonly ICartStorage _cartStorage;
    private readonly IProductRepository _productRepository;

    public CartService(ICartStorage cartStorage, IProductRepository productRepository)
    {
        _cartStorage = cartStorage;
        _productRepository = productRepository;
    }

    public Cart GetCart() => _cartStorage.GetCart();

    public async Task<Cart> AddItemAsync(AddToCartDto dto)
    {
        var product = await _productRepository.GetByIdAsync(dto.ProductId);
        if (product == null)
            throw new InvalidOperationException("Product not found.");

        var cart = _cartStorage.GetCart();

        var existing = cart.Items.FirstOrDefault(i =>
            i.ProductId == dto.ProductId &&
            i.Weight == dto.Weight &&
            i.Grind == dto.Grind);

        if (existing != null)
        {
            existing.Quantity += dto.Quantity;
        }
        else
        {
            var priceAdjustment = 0m;
            if (dto.Weight != null)
            {
                var weightOption = product.WeightOptions.FirstOrDefault(w => w.Weight == dto.Weight);
                if (weightOption != null)
                    priceAdjustment = weightOption.PriceAdjustment;
            }

            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSlug = product.Slug,
                ImageUrl = product.ImageUrl,
                UnitPrice = product.Price,
                Quantity = dto.Quantity,
                Weight = dto.Weight,
                Grind = dto.Grind,
                PriceAdjustment = priceAdjustment
            });
        }

        _cartStorage.SaveCart(cart);
        return cart;
    }

    public Cart UpdateItemQuantity(int productId, int quantity)
    {
        var cart = _cartStorage.GetCart();
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            if (quantity <= 0)
                cart.Items.Remove(item);
            else
                item.Quantity = quantity;
        }

        _cartStorage.SaveCart(cart);
        return cart;
    }

    public Cart RemoveItem(int productId)
    {
        var cart = _cartStorage.GetCart();
        cart.Items.RemoveAll(i => i.ProductId == productId);
        _cartStorage.SaveCart(cart);
        return cart;
    }

    public void ClearCart()
    {
        _cartStorage.ClearCart();
    }

    public int GetItemCount()
    {
        return _cartStorage.GetCart().TotalItems;
    }
}
