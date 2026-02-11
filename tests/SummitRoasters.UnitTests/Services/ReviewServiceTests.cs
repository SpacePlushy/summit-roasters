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

    // TODO: SubmitReview_CreatesReview - verify that SubmitReviewAsync creates a review with the correct ProductId, UserId, Rating, Title, and Body when the user has not already reviewed the product

    // TODO: SubmitReview_ThrowsWhenAlreadyReviewed - verify that SubmitReviewAsync throws an InvalidOperationException when the user has already reviewed the product (ExistsForUserAndProductAsync returns true)

    // TODO: SubmitReview_SetsVerifiedPurchaseFlag - verify that SubmitReviewAsync sets IsVerifiedPurchase to true when the user has purchased the product (UserHasPurchasedProductAsync returns true) and false otherwise

    // TODO: SubmitReview_ClampsRating - verify that SubmitReviewAsync clamps the rating to the 1-5 range (e.g., a rating of 0 becomes 1, a rating of 10 becomes 5)

    // TODO: GetRatingBreakdown_ReturnsCorrectCounts - verify that GetRatingBreakdownAsync returns a dictionary with keys 1-5 and correct counts for each rating level based on the reviews returned by the repository

    // TODO: HasUserReviewed - verify that HasUserReviewedAsync delegates to IReviewRepository.ExistsForUserAndProductAsync and returns the correct boolean result
}
