using FluentAssertions;
using Moq;
using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Services;

namespace SummitRoasters.UnitTests.Services;

public class CartServiceTests
{
    private readonly Mock<ICartStorage> _cartStorageMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly CartService _cartService;
    private Cart _cart;

    public CartServiceTests()
    {
        _cartStorageMock = new Mock<ICartStorage>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _cart = new Cart();
        _cartStorageMock.Setup(s => s.GetCart()).Returns(() => _cart);
        _cartStorageMock.Setup(s => s.SaveCart(It.IsAny<Cart>())).Callback<Cart>(c => _cart = c);
        _cartStorageMock.Setup(s => s.ClearCart()).Callback(() => _cart = new Cart());

        _cartService = new CartService(_cartStorageMock.Object, _productRepositoryMock.Object);
    }

    [Fact]
    public async Task AddItem_AddsNewItemToCart()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Summit Blend",
            Slug = "summit-blend",
            Price = 16.99m,
            StockQuantity = 10,
            WeightOptions = new List<WeightOption>()
        };
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        var dto = new AddToCartDto { ProductId = 1, Quantity = 1 };

        // Act
        var result = await _cartService.AddItemAsync(dto);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items[0].ProductName.Should().Be("Summit Blend");
        result.Items[0].UnitPrice.Should().Be(16.99m);
    }

    [Fact]
    public void RemoveItem_RemovesFromCart()
    {
        // Arrange
        _cart.Items.Add(new CartItem
        {
            ProductId = 1,
            ProductName = "Summit Blend",
            UnitPrice = 16.99m,
            Quantity = 1
        });

        // Act
        var result = _cartService.RemoveItem(1);

        // Assert
        result.Items.Should().HaveCount(0);
    }

    [Fact]
    public void UpdateItemQuantity_UpdatesQuantity()
    {
        // Arrange
        _cart.Items.Add(new CartItem
        {
            ProductId = 1,
            ProductName = "Summit Blend",
            UnitPrice = 16.99m,
            Quantity = 1
        });

        // Act
        var result = _cartService.UpdateItemQuantity(1, 3);

        // Assert
        result.Items[0].Quantity.Should().Be(3);
    }

    [Fact]
    public async Task AddItem_IncrementsQuantity_WhenSameProductExists()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Summit Blend",
            Slug = "summit-blend",
            Price = 16.99m,
            StockQuantity = 10,
            WeightOptions = new List<WeightOption>()
        };
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        var dto = new AddToCartDto { ProductId = 1, Quantity = 1 };

        // Add first item
        await _cartService.AddItemAsync(dto);

        // Act - add same product again
        var result = await _cartService.AddItemAsync(dto);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items[0].Quantity.Should().Be(2);
    }

    [Fact]
    public async Task AddItem_ThrowsWhenProductNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Product?)null);

        var dto = new AddToCartDto { ProductId = 999, Quantity = 1 };

        // Act
        var act = () => _cartService.AddItemAsync(dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*not found*");
    }

    [Fact]
    public void UpdateItemQuantity_RemovesWhenZero()
    {
        // Arrange
        _cart.Items.Add(new CartItem
        {
            ProductId = 1,
            ProductName = "Summit Blend",
            UnitPrice = 16.99m,
            Quantity = 2
        });

        // Act
        var result = _cartService.UpdateItemQuantity(1, 0);

        // Assert
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public void ClearCart_EmptiesCart()
    {
        // Arrange
        _cart.Items.Add(new CartItem
        {
            ProductId = 1,
            ProductName = "Summit Blend",
            UnitPrice = 16.99m,
            Quantity = 1
        });

        // Act
        _cartService.ClearCart();

        // Assert
        _cartStorageMock.Verify(s => s.ClearCart(), Times.Once);
        _cart.Items.Should().BeEmpty();
    }

    [Fact]
    public void GetItemCount_ReturnsSumOfQuantities()
    {
        // Arrange
        _cart.Items.Add(new CartItem { ProductId = 1, ProductName = "A", UnitPrice = 10m, Quantity = 2 });
        _cart.Items.Add(new CartItem { ProductId = 2, ProductName = "B", UnitPrice = 15m, Quantity = 3 });

        // Act
        var result = _cartService.GetItemCount();

        // Assert
        result.Should().Be(5);
    }
}
