using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.DTOs;
using SummitRoasters.Core.Interfaces.Services;

namespace SummitRoasters.Web.Controllers.Api;

[Route("api/reviews")]
[ApiController]
[Authorize]
public class ReviewsApiController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsApiController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var userName = User.Identity?.Name ?? "Anonymous";

        dto.UserId = userId;

        var hasReviewed = await _reviewService.HasUserReviewedAsync(userId, dto.ProductId);
        if (hasReviewed)
            return BadRequest(new { error = "You have already reviewed this product." });

        try
        {
            var review = await _reviewService.SubmitReviewAsync(dto);

            return Ok(new
            {
                id = review.Id,
                userName = review.UserName,
                rating = review.Rating,
                title = review.Title,
                body = review.Body,
                isVerifiedPurchase = review.IsVerifiedPurchase,
                createdAt = review.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
