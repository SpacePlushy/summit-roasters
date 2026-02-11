using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Interfaces.Services;

public interface ISearchService
{
    Task<List<Product>> SearchAsync(string query, int maxResults = 20);
    Task<List<Product>> AutocompleteAsync(string query, int maxResults = 5);
}
