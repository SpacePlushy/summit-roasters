using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Interfaces.Services;
using SummitRoasters.Core.Models;

namespace SummitRoasters.Core.Services;

public class SearchService : ISearchService
{
    private readonly IProductRepository _productRepository;

    public SearchService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> SearchAsync(string query, int maxResults = 20)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<Product>();

        return await _productRepository.SearchAsync(query.Trim(), maxResults);
    }

    public async Task<List<Product>> AutocompleteAsync(string query, int maxResults = 5)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
            return new List<Product>();

        return await _productRepository.SearchAsync(query.Trim(), maxResults);
    }
}
