namespace SummitRoasters.Web.ViewModels;

public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    public string BaseUrl { get; set; } = string.Empty;
    public Dictionary<string, string> RouteValues { get; set; } = new();

    // Aliases used by views
    public bool HasPreviousPage => HasPrevious;
    public bool HasNextPage => HasNext;

    // URL generator for pagination links
    public string GetPageUrl(int page)
    {
        var routeValues = new Dictionary<string, string>(RouteValues)
        {
            ["page"] = page.ToString()
        };
        var queryString = string.Join("&", routeValues.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
        return $"{BaseUrl}?{queryString}";
    }
}
