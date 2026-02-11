using FluentAssertions;
using Moq;
using SummitRoasters.Core.Interfaces.Repositories;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Services;

namespace SummitRoasters.UnitTests.Services;

public class SearchServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly SearchService _searchService;

    public SearchServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _searchService = new SearchService(_productRepositoryMock.Object);
    }

    // TODO: Search_ReturnsResults - verify that SearchAsync returns products from the repository when a valid query is provided

    // TODO: Search_ReturnsEmpty_WhenBlankQuery - verify that SearchAsync returns an empty list when the query is null, empty, or whitespace without calling the repository

    // TODO: Autocomplete_ReturnsResults - verify that AutocompleteAsync returns products from the repository when a valid query of 2 or more characters is provided

    // TODO: Autocomplete_ReturnsEmpty_WhenQueryTooShort - verify that AutocompleteAsync returns an empty list when the query is fewer than 2 characters without calling the repository
}
