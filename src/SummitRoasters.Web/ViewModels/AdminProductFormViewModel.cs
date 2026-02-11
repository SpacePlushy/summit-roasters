using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Web.ViewModels;

public class AdminProductFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [StringLength(5000)]
    public string Description { get; set; } = string.Empty;

    [StringLength(10000)]
    [Display(Name = "Full Description")]
    public string? FullDescription { get; set; }

    [Required]
    [Range(0.01, 99999.99)]
    public decimal Price { get; set; }

    [Range(0.01, 99999.99)]
    [Display(Name = "Compare At Price")]
    public decimal? CompareAtPrice { get; set; }

    [Required]
    public Category Category { get; set; }

    [Display(Name = "Roast Level")]
    public RoastLevel? RoastLevel { get; set; }

    [StringLength(100)]
    public string? Origin { get; set; }

    [StringLength(100)]
    public string? Region { get; set; }

    [StringLength(100)]
    public string? Altitude { get; set; }

    [StringLength(100)]
    public string? Process { get; set; }

    [StringLength(100)]
    [Display(Name = "Process Method")]
    public string? ProcessMethod
    {
        get => Process;
        set => Process = value;
    }

    [Display(Name = "Flavor Notes (comma-separated)")]
    public string? FlavorNotesInput { get; set; }

    public List<string>? FlavorNotes { get; set; }

    [StringLength(500)]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

    // Select list items for dropdowns
    public IEnumerable<SelectListItem>? CategoryOptions { get; set; }
    public IEnumerable<SelectListItem>? RoastLevelOptions { get; set; }
}
