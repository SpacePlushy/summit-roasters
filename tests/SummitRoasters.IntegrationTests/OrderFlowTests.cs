namespace SummitRoasters.IntegrationTests;

public class OrderFlowTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public OrderFlowTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    // TODO: FullCheckoutFlow_CreatesOrder
    // TODO: Checkout_RequiresAuthentication
    // TODO: OrderHistory_ShowsUserOrders
    // TODO: OrderDetail_ShowsCorrectOrder
}
