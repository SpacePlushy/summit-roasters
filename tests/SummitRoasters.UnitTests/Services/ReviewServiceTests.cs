using FluentAssertions;
using Moq;
using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Services;

namespace SummitRoasters.UnitTests.Services;

public class ReviewServiceTests
{
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly ReviewService _reviewService;

    public ReviewServiceTests()
    {
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _reviewService = new ReviewService(_reviewRepositoryMock.Object, _orderRepositoryMock.Object);
    }

    [Fact]
    public async Task SubmitReview_CreatesReview()
    {
        // Arrange
        var dto = new CreateReviewDto
        {
            ProductId = 1,
            UserId = "user-1",
            Rating = 4,
            Title = "Great coffee",
            Body = "Really enjoyed it"
        };
        _reviewRepositoryMock
            .Setup(r => r.ExistsForUserAndProductAsync("user-1", 1))
            .ReturnsAsync(false);
        _orderRepositoryMock
            .Setup(r => r.UserHasPurchasedProductAsync("user-1", 1))
            .ReturnsAsync(false);
        _reviewRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Review>()))
            .ReturnsAsync((Review r) => r);

        // Act
        var result = await _reviewService.SubmitReviewAsync(dto);

        // Assert
        result.ProductId.Should().Be(1);
        result.UserId.Should().Be("user-1");
        result.Rating.Should().Be(4);
        result.Title.Should().Be("Great coffee");
        result.Body.Should().Be("Really enjoyed it");
    }

    [Fact]
    public async Task SubmitReview_ThrowsWhenAlreadyReviewed()
    {
        // Arrange
        var dto = new CreateReviewDto
        {
            ProductId = 1,
            UserId = "user-1",
            Rating = 5,
            Title = "Duplicate",
            Body = "Should fail"
        };
        _reviewRepositoryMock
            .Setup(r => r.ExistsForUserAndProductAsync("user-1", 1))
            .ReturnsAsync(true);

        // Act
        var act = () => _reviewService.SubmitReviewAsync(dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already reviewed*");
    }

    [Fact]
    public async Task SubmitReview_SetsVerifiedPurchaseFlag()
    {
        // Arrange
        var dto = new CreateReviewDto
        {
            ProductId = 1,
            UserId = "user-1",
            Rating = 5,
            Title = "Verified",
            Body = "I bought this"
        };
        _reviewRepositoryMock
            .Setup(r => r.ExistsForUserAndProductAsync("user-1", 1))
            .ReturnsAsync(false);
        _orderRepositoryMock
            .Setup(r => r.UserHasPurchasedProductAsync("user-1", 1))
            .ReturnsAsync(true);
        _reviewRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Review>()))
            .ReturnsAsync((Review r) => r);

        // Act
        var result = await _reviewService.SubmitReviewAsync(dto);

        // Assert
        result.IsVerifiedPurchase.Should().BeTrue();
    }

    [Fact]
    public async Task SubmitReview_ClampsRating()
    {
        // Arrange
        _reviewRepositoryMock
            .Setup(r => r.ExistsForUserAndProductAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);
        _orderRepositoryMock
            .Setup(r => r.UserHasPurchasedProductAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);
        _reviewRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Review>()))
            .ReturnsAsync((Review r) => r);

        var dtoLow = new CreateReviewDto { ProductId = 1, UserId = "u1", Rating = 0, Title = "Low", Body = "Too low" };
        var dtoHigh = new CreateReviewDto { ProductId = 2, UserId = "u2", Rating = 10, Title = "High", Body = "Too high" };

        // Act
        var resultLow = await _reviewService.SubmitReviewAsync(dtoLow);
        var resultHigh = await _reviewService.SubmitReviewAsync(dtoHigh);

        // Assert
        resultLow.Rating.Should().Be(1);
        resultHigh.Rating.Should().Be(5);
    }

    [Fact]
    public async Task GetRatingBreakdown_ReturnsCorrectCounts()
    {
        // Arrange
        var reviews = new List<Review>
        {
            new Review { Rating = 5 },
            new Review { Rating = 5 },
            new Review { Rating = 4 },
            new Review { Rating = 3 },
            new Review { Rating = 1 }
        };
        _reviewRepositoryMock
            .Setup(r => r.GetByProductIdAsync(1, 1, int.MaxValue))
            .ReturnsAsync((reviews, reviews.Count));

        // Act
        var result = await _reviewService.GetRatingBreakdownAsync(1);

        // Assert
        result[5].Should().Be(2);
        result[4].Should().Be(1);
        result[3].Should().Be(1);
        result[2].Should().Be(0);
        result[1].Should().Be(1);
    }

    [Fact]
    public async Task HasUserReviewed_DelegatesToRepository()
    {
        // Arrange
        _reviewRepositoryMock
            .Setup(r => r.ExistsForUserAndProductAsync("user-1", 1))
            .ReturnsAsync(true);

        // Act
        var result = await _reviewService.HasUserReviewedAsync("user-1", 1);

        // Assert
        result.Should().BeTrue();
        _reviewRepositoryMock.Verify(r => r.ExistsForUserAndProductAsync("user-1", 1), Times.Once);
    }
}
