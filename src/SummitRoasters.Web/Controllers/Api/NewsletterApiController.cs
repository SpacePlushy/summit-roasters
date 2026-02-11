using Microsoft.AspNetCore.Mvc;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Web.Controllers.Api;

[Route("api/newsletter")]
[ApiController]
public class NewsletterApiController : ControllerBase
{
    private readonly INewsletterRepository _newsletterRepository;

    public NewsletterApiController(INewsletterRepository newsletterRepository)
    {
        _newsletterRepository = newsletterRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe([FromBody] NewsletterSubscribeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(new { error = "Email is required." });

        var alreadySubscribed = await _newsletterRepository.ExistsByEmailAsync(request.Email);
        if (alreadySubscribed)
            return Ok(new { message = "You are already subscribed to our newsletter." });

        var subscription = new NewsletterSubscription
        {
            Email = request.Email
        };

        await _newsletterRepository.AddAsync(subscription);

        return Ok(new { message = "Successfully subscribed to our newsletter!" });
    }
}

public class NewsletterSubscribeRequest
{
    public string Email { get; set; } = string.Empty;
}
