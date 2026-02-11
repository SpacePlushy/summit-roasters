# Summit Roasters Architecture

## Clean Architecture Layers
```
Core (no deps) → Infrastructure (depends on Core) → Web (depends on both)
```

### Core (`src/SummitRoasters.Core/`)
- **Models/Enums/**: Category, RoastLevel, GrindType, OrderStatus, PaymentStatus, DiscountType
- **Models/**: Product, WeightOption, ApplicationUser (extends IdentityUser), ShippingAddress, Cart/CartItem (session POCOs), Order, OrderItem, Review, NewsletterSubscription, DiscountCode
- **DTOs/**: ProductFilterDto, AddToCartDto, CreateReviewDto, ShippingAddressDto, DiscountResult, InventoryStatus
- **Interfaces/Repositories/**: IProductRepository, IOrderRepository, IReviewRepository, IDiscountCodeRepository, INewsletterRepository, IShippingAddressRepository
- **Interfaces/Services/**: IPricingService, IInventoryService, ICartStorage, ICartService, IOrderService, ISearchService, IReviewService
- **Services/**: PricingService, InventoryService, CartService, OrderService, SearchService, ReviewService

### Infrastructure (`src/SummitRoasters.Infrastructure/`)
- **Data/**: ApplicationDbContext (IdentityDbContext<ApplicationUser>), DataSeeder, EF configurations
- **Repositories/**: All 6 repository implementations
- **DependencyInjection.cs**: `AddInfrastructure()` extension method

### Web (`src/SummitRoasters.Web/`)
- **Program.cs**: Composition root (Identity, Session, DI, seeding, middleware)
- **Services/SessionCartStorage.cs**: ICartStorage implementation using ISession
- **ViewModels/**: 27 ViewModel classes
- **Controllers/**: 7 MVC controllers + 4 API controllers
- **Views/**: 37 Razor views with Tailwind styling and data-testid attributes
- **wwwroot/js/**: 9 vanilla JS files (site, cart, search, product-detail, checkout, review, newsletter, admin/product-form, admin/order-status)

## Key Patterns
- Cart: Session-based via `ICartStorage` abstraction
- Pagination: Repositories return `(List<T>, int TotalCount)` tuples
- Product lookup: By slug for public URLs, by ID for admin
- Order numbers: `SR-{date}-{guid8}` format
- Status transitions: Validated dictionary in OrderService
- Reviews: One per user per product, verified purchase flag auto-set

## Routes
- `/` - Home (featured + new arrivals)
- `/products` - Product listing with filters
- `/products/{slug}` - Product detail
- `/search?q=` - Search results
- `/cart` - Cart page
- `/account/*` - Login, Register, Orders, Settings
- `/checkout/*` - Checkout flow (authorized)
- `/admin/*` - Admin dashboard, products CRUD, orders (Admin role)
- `/api/cart/*` - Cart AJAX endpoints
- `/api/products/search` - Autocomplete
- `/api/reviews` - Submit review
- `/api/newsletter` - Newsletter signup
