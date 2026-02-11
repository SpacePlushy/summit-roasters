using System.ComponentModel.DataAnnotations;

namespace SummitRoasters.Web.ViewModels;

public class WriteReviewViewModel
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Body { get; set; } = string.Empty;
}
