namespace SummitRoasters.Core.DTOs;

public class ShippingAddressDto
{
    public int? Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = "US";
    public string? Phone { get; set; }
    public bool IsDefault { get; set; }
}
