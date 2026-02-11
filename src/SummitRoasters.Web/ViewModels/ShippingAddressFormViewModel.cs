using System.ComponentModel.DataAnnotations;

namespace SummitRoasters.Web.ViewModels;

public class ShippingAddressFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    [Display(Name = "Address Line 1")]
    public string AddressLine1 { get; set; } = string.Empty;

    [StringLength(200)]
    [Display(Name = "Address Line 2")]
    public string? AddressLine2 { get; set; }

    [Required]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string State { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Display(Name = "ZIP Code")]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    [StringLength(2)]
    public string Country { get; set; } = "US";

    [StringLength(20)]
    [Phone]
    public string? Phone { get; set; }

    [Display(Name = "Default Address")]
    public bool IsDefault { get; set; }

    // Aliases used by views
    public string Street
    {
        get => AddressLine1;
        set => AddressLine1 = value;
    }

    public string? Street2
    {
        get => AddressLine2;
        set => AddressLine2 = value;
    }
}
