using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SummitRoasters.Core.Models;
using SummitRoasters.Core.Models.Enums;

namespace SummitRoasters.Infrastructure.Data;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DataSeeder(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        if (await _context.Products.AnyAsync())
            return;

        await SeedRolesAsync();
        var users = await SeedUsersAsync();
        var products = await SeedProductsAsync();
        await SeedReviewsAsync(products, users);
        await SeedOrdersAsync(products, users);
        await SeedDiscountCodesAsync();
    }

    private async Task SeedRolesAsync()
    {
        foreach (var role in new[] { "Admin", "Customer" })
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private async Task<Dictionary<string, ApplicationUser>> SeedUsersAsync()
    {
        var users = new Dictionary<string, ApplicationUser>();

        var admin = new ApplicationUser
        {
            UserName = "admin@summitroasters.com",
            Email = "admin@summitroasters.com",
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true
        };
        await _userManager.CreateAsync(admin, "Admin123!");
        await _userManager.AddToRoleAsync(admin, "Admin");
        users["admin"] = admin;

        var sarah = new ApplicationUser
        {
            UserName = "sarah@example.com",
            Email = "sarah@example.com",
            FirstName = "Sarah",
            LastName = "Johnson",
            EmailConfirmed = true
        };
        await _userManager.CreateAsync(sarah, "Customer123!");
        await _userManager.AddToRoleAsync(sarah, "Customer");
        users["sarah"] = sarah;

        var mike = new ApplicationUser
        {
            UserName = "mike@example.com",
            Email = "mike@example.com",
            FirstName = "Mike",
            LastName = "Chen",
            EmailConfirmed = true
        };
        await _userManager.CreateAsync(mike, "Customer123!");
        await _userManager.AddToRoleAsync(mike, "Customer");
        users["mike"] = mike;

        // Add shipping addresses
        _context.ShippingAddresses.AddRange(
            new ShippingAddress { UserId = sarah.Id, FullName = "Sarah Johnson", AddressLine1 = "123 Coffee Lane", City = "Portland", State = "OR", ZipCode = "97201", IsDefault = true },
            new ShippingAddress { UserId = mike.Id, FullName = "Mike Chen", AddressLine1 = "456 Roast Ave", City = "Seattle", State = "WA", ZipCode = "98101", IsDefault = true }
        );
        await _context.SaveChangesAsync();

        return users;
    }

    private async Task<List<Product>> SeedProductsAsync()
    {
        var products = new List<Product>
        {
            // Single Origins (10)
            new() { Name = "Ethiopian Yirgacheffe", Slug = "ethiopian-yirgacheffe", Description = "A bright and complex coffee from the birthplace of coffee. Grown at high altitude in the Gedeo Zone, this natural process coffee delivers an explosion of blueberry and citrus flavors with a wine-like body.", Price = 18.99m, Category = Category.SingleOrigin, RoastLevel = RoastLevel.Light, Origin = "Ethiopia", Region = "Yirgacheffe, Gedeo Zone", Altitude = "1,700-2,200m", Process = "Natural", FlavorNotes = "Blueberry, Citrus, Jasmine, Wine", ImageUrl = "/images/products/ethiopian-yirgacheffe.png", StockQuantity = 45, IsFeatured = true },
            new() { Name = "Colombian Supremo", Slug = "colombian-supremo", Description = "A classic Colombian coffee with a smooth, well-balanced profile. Sourced from family farms in the Huila department, this washed coffee offers rich chocolate notes with a clean finish.", Price = 16.99m, Category = Category.SingleOrigin, RoastLevel = RoastLevel.Medium, Origin = "Colombia", Region = "Huila", Altitude = "1,500-1,800m", Process = "Washed", FlavorNotes = "Chocolate, Caramel, Nutty, Clean", ImageUrl = "/images/products/colombian-supremo.png", StockQuantity = 60, IsFeatured = true },
            new() { Name = "Guatemalan Antigua", Slug = "guatemalan-antigua", Description = "Grown in the shadow of three volcanoes, this coffee benefits from rich volcanic soil. Full-bodied with a spicy complexity and a smooth chocolate finish.", Price = 17.99m, Category = Category.SingleOrigin, RoastLevel = RoastLevel.MediumDark, Origin = "Guatemala", Region = "Antigua", Altitude = "1,500-1,700m", Process = "Washed", FlavorNotes = "Dark Chocolate, Spice, Smoke, Full Body", ImageUrl = "/images/products/guatemalan-antigua.png", StockQuantity = 35 },
            new() { Name = "Kenyan AA", Slug = "kenyan-aa", Description = "Kenya's finest grade of coffee, known for its bold, wine-like acidity and complex flavor profile. Grown on the slopes of Mount Kenya with meticulous processing.", Price = 21.99m, Category = Category.SingleOrigin, RoastLevel = RoastLevel.MediumLight, Origin = "Kenya", Region = "Nyeri", Altitude = "1,700-2,000m", Process = "Washed", FlavorNotes = "Blackcurrant, Grapefruit, Tomato, Brown Sugar", ImageUrl = "/images/products/kenyan-aa.png", StockQuantity = 25, IsFeatured = true },
            new() { Name = "Sumatra Mandheling", Slug = "sumatra-mandheling", Description = "A distinctive Indonesian coffee with an earthy, full-bodied character. The wet-hull processing method gives it unique herbal and earthy qualities with low acidity.", Price = 17.49m, Category = Category.SingleOrigin, RoastLevel = RoastLevel.Dark, Origin = "Indonesia", Region = "Mandheling, Sumatra", Altitude = "1,100-1,600m", Process = "Wet Hulled", FlavorNotes = "Earthy, Herbal, Dark Chocolate, Tobacco", ImageUrl = "/images/products/sumatra-mandheling.png", StockQuantity = 40 },
            new() { Name = "Costa Rican Tarrazú", Slug = "costa-rican-tarrazu", Description = "From the renowned Tarrazú region, this honey-processed coffee offers a sweet, syrupy body with bright citrus acidity. A perfect everyday specialty coffee.", Price = 19.49m, Category = Category.SingleOrigin, RoastLevel = RoastLevel.Medium, Origin = "Costa Rica", Region = "Tarrazú", Altitude = "1,200-1,800m", Process = "Honey", FlavorNotes = "Honey, Orange, Brown Sugar, Clean", ImageUrl = "/images/products/costa-rican-tarrazu.png", StockQuantity = 30 },
            new() { Name = "Panama Geisha", Slug = "panama-geisha", Description = "The pinnacle of specialty coffee. This rare Geisha variety from Boquete delivers an extraordinary floral and tea-like experience that redefines what coffee can be.", Price = 45.99m, CompareAtPrice = 55.99m, Category = Category.SingleOrigin, RoastLevel = RoastLevel.Light, Origin = "Panama", Region = "Boquete", Altitude = "1,600-1,900m", Process = "Washed", FlavorNotes = "Jasmine, Bergamot, Peach, Tropical", ImageUrl = "/images/products/panama-geisha.png", StockQuantity = 8, IsFeatured = true },
            new() { Name = "Brazilian Santos", Slug = "brazilian-santos", Description = "A smooth, low-acidity coffee from Brazil's Santos region. Naturally processed with sweet, nutty characteristics perfect for espresso blends or a comforting cup.", Price = 14.99m, Category = Category.SingleOrigin, RoastLevel = RoastLevel.Medium, Origin = "Brazil", Region = "Santos, Minas Gerais", Altitude = "800-1,200m", Process = "Natural", FlavorNotes = "Nuts, Chocolate, Low Acidity, Smooth", ImageUrl = "/images/products/brazilian-santos.png", StockQuantity = 80 },
            new() { Name = "Rwandan Bourbon", Slug = "rwandan-bourbon", Description = "A stunning African coffee with bright, complex fruit flavors. The Bourbon variety thrives at high altitude producing exceptional sweetness and clarity.", Price = 19.99m, Category = Category.SingleOrigin, RoastLevel = RoastLevel.MediumLight, Origin = "Rwanda", Region = "Nyamasheke", Altitude = "1,800-2,100m", Process = "Washed", FlavorNotes = "Red Apple, Plum, Floral, Tea-like", ImageUrl = "/images/products/rwandan-bourbon.png", StockQuantity = 20 },
            new() { Name = "Jamaican Blue Mountain", Slug = "jamaican-blue-mountain", Description = "One of the world's most prestigious coffees, grown in Jamaica's Blue Mountains. Smooth, mild, and remarkably balanced with virtually no bitterness.", Price = 54.99m, Category = Category.SingleOrigin, RoastLevel = RoastLevel.Medium, Origin = "Jamaica", Region = "Blue Mountains", Altitude = "900-1,500m", Process = "Washed", FlavorNotes = "Floral, Sweet Herbs, Mild, Clean", ImageUrl = "/images/products/jamaican-blue-mountain.png", StockQuantity = 5 },

            // Blends (6)
            new() { Name = "Summit Sunrise Blend", Slug = "summit-sunrise-blend", Description = "Our signature morning blend combining bright Central American coffees with smooth Brazilian base. Designed to start your day with balanced sweetness and clean energy.", Price = 15.99m, Category = Category.Blend, RoastLevel = RoastLevel.Medium, FlavorNotes = "Caramel, Citrus, Smooth, Balanced", ImageUrl = "/images/products/summit-sunrise-blend.png", StockQuantity = 100, IsFeatured = true },
            new() { Name = "Basecamp Espresso", Slug = "basecamp-espresso", Description = "A bold, syrupy espresso blend built for intensity. Dark chocolate and caramel sweetness with enough body to cut through milk for perfect lattes.", Price = 17.99m, Category = Category.Blend, RoastLevel = RoastLevel.MediumDark, FlavorNotes = "Dark Chocolate, Caramel, Syrupy, Bold", ImageUrl = "/images/products/basecamp-espresso.png", StockQuantity = 75 },
            new() { Name = "Alpine Dark Roast", Slug = "alpine-dark-roast", Description = "For those who love their coffee bold and smoky. A full-bodied dark roast with rich, bittersweet chocolate notes and a lingering smoky finish.", Price = 14.99m, Category = Category.Blend, RoastLevel = RoastLevel.Dark, FlavorNotes = "Smoky, Bittersweet, Bold, Full Body", ImageUrl = "/images/products/alpine-dark-roast.png", StockQuantity = 65 },
            new() { Name = "Trailhead Breakfast Blend", Slug = "trailhead-breakfast-blend", Description = "A light and lively breakfast blend with sweet fruit notes and a crisp, clean finish. Perfect for pour-over or drip brewing.", Price = 13.99m, Category = Category.Blend, RoastLevel = RoastLevel.Light, FlavorNotes = "Fruit, Sweet, Crisp, Clean", ImageUrl = "/images/products/trailhead-breakfast-blend.png", StockQuantity = 55 },
            new() { Name = "Campfire Blend", Slug = "campfire-blend", Description = "Our coziest blend, evoking warm nights by the fire. Smoky and sweet with marshmallow-like softness and deep chocolate undertones.", Price = 16.49m, Category = Category.Blend, RoastLevel = RoastLevel.MediumDark, FlavorNotes = "S'mores, Smoky, Sweet, Cozy", ImageUrl = "/images/products/campfire-blend.png", StockQuantity = 40, IsFeatured = true },
            new() { Name = "Peak Performance Blend", Slug = "peak-performance-blend", Description = "Our highest-caffeine blend for those who need extra fuel. A carefully crafted combination of high-caffeine varietals with surprising smoothness.", Price = 18.99m, Category = Category.Blend, RoastLevel = RoastLevel.Medium, FlavorNotes = "Bold, Nutty, Energizing, Smooth", ImageUrl = "/images/products/peak-performance-blend.png", StockQuantity = 50 },

            // Decaf (4)
            new() { Name = "Swiss Water Decaf Colombian", Slug = "swiss-water-decaf-colombian", Description = "All the flavor of our Colombian Supremo without the caffeine. Swiss Water Process preserves the rich chocolate and caramel notes you love.", Price = 17.99m, Category = Category.Decaf, RoastLevel = RoastLevel.Medium, Origin = "Colombia", FlavorNotes = "Chocolate, Caramel, Smooth, Decaf", ImageUrl = "/images/products/swiss-water-decaf-colombian.png", StockQuantity = 30 },
            new() { Name = "Decaf Midnight Blend", Slug = "decaf-midnight-blend", Description = "A full-bodied decaf dark roast for evening enjoyment. Rich and satisfying with dark chocolate and toasted almond notes.", Price = 16.99m, Category = Category.Decaf, RoastLevel = RoastLevel.Dark, FlavorNotes = "Dark Chocolate, Toasted Almond, Full Body", ImageUrl = "/images/products/decaf-midnight-blend.png", StockQuantity = 25 },
            new() { Name = "Decaf Ethiopian", Slug = "decaf-ethiopian", Description = "Our Ethiopian Yirgacheffe decaffeinated using the mountain water process. Retains the bright, fruity character with none of the caffeine.", Price = 20.99m, Category = Category.Decaf, RoastLevel = RoastLevel.Light, Origin = "Ethiopia", FlavorNotes = "Berry, Citrus, Floral, Bright", ImageUrl = "/images/products/decaf-ethiopian.png", StockQuantity = 15 },
            new() { Name = "Decaf Espresso Blend", Slug = "decaf-espresso-blend", Description = "Specially crafted for decaf espresso lovers. Sweet, creamy, and full-bodied enough to shine through milk-based drinks.", Price = 18.49m, Category = Category.Decaf, RoastLevel = RoastLevel.MediumDark, FlavorNotes = "Caramel, Cream, Sweet, Balanced", ImageUrl = "/images/products/decaf-espresso-blend.png", StockQuantity = 20 },

            // Equipment (6)
            new() { Name = "Summit Pour-Over Dripper", Slug = "summit-pour-over-dripper", Description = "Our signature ceramic pour-over dripper with a unique ridged design for optimal extraction. Brews a clean, flavorful cup every time. Includes 50 paper filters.", Price = 34.99m, Category = Category.Equipment, ImageUrl = "/images/products/summit-pour-over-dripper.png", StockQuantity = 25 },
            new() { Name = "Basecamp French Press", Slug = "basecamp-french-press", Description = "A double-walled stainless steel French press that keeps coffee hot for hours. 34oz capacity perfect for sharing. Durable enough for any adventure.", Price = 44.99m, Category = Category.Equipment, ImageUrl = "/images/products/basecamp-french-press.png", StockQuantity = 20 },
            new() { Name = "Summit Burr Grinder", Slug = "summit-burr-grinder", Description = "A conical burr grinder with 15 grind settings from espresso-fine to French press coarse. Consistent grind size for optimal extraction.", Price = 79.99m, Category = Category.Equipment, ImageUrl = "/images/products/summit-burr-grinder.png", StockQuantity = 15, IsFeatured = true },
            new() { Name = "Trail Kettle - Gooseneck", Slug = "trail-kettle-gooseneck", Description = "A precision gooseneck kettle with built-in thermometer. Variable flow rate control for perfect pour-over technique. Heats quickly on any stovetop.", Price = 54.99m, Category = Category.Equipment, ImageUrl = "/images/products/trail-kettle-gooseneck.png", StockQuantity = 18 },
            new() { Name = "AeroPress Go", Slug = "aeropress-go", Description = "The compact travel version of the beloved AeroPress. Makes smooth, rich coffee anywhere. Includes travel mug, filters, and stirrer in a self-contained kit.", Price = 39.99m, Category = Category.Equipment, ImageUrl = "/images/placeholder-equipment.svg", StockQuantity = 30 },
            new() { Name = "Cold Brew Maker", Slug = "cold-brew-maker", Description = "A sleek glass cold brew maker with fine mesh filter. Makes 1.5L of smooth, concentrated cold brew. Simply steep overnight for perfect results.", Price = 29.99m, Category = Category.Equipment, ImageUrl = "/images/placeholder-equipment.svg", StockQuantity = 22 },

            // Accessories (4)
            new() { Name = "Summit Coffee Mug", Slug = "summit-coffee-mug", Description = "A handcrafted ceramic mug with our mountain logo. 12oz capacity, microwave and dishwasher safe. Available in espresso brown glaze.", Price = 16.99m, Category = Category.Accessories, ImageUrl = "/images/placeholder-equipment.svg", StockQuantity = 50 },
            new() { Name = "Coffee Scale with Timer", Slug = "coffee-scale-timer", Description = "A precision 0.1g scale with built-in timer for consistent brewing. Auto-tare, auto-off, and USB rechargeable. Essential for any home barista.", Price = 24.99m, Category = Category.Accessories, ImageUrl = "/images/placeholder-equipment.svg", StockQuantity = 35 },
            new() { Name = "Canvas Tote Bag", Slug = "canvas-tote-bag", Description = "A durable organic cotton tote featuring the Summit Roasters mountain design. Perfect for farmers market runs and coffee shop visits.", Price = 12.99m, Category = Category.Accessories, ImageUrl = "/images/placeholder-equipment.svg", StockQuantity = 40 },
            new() { Name = "Pour-Over Filter Pack (100)", Slug = "pour-over-filter-pack", Description = "Premium unbleached paper filters compatible with our Summit Pour-Over Dripper and most standard drippers. 100 count.", Price = 8.99m, Category = Category.Accessories, ImageUrl = "/images/placeholder-equipment.svg", StockQuantity = 100 },
        };

        // Add weight options for coffee products
        foreach (var product in products.Where(p => p.Category is Category.SingleOrigin or Category.Blend or Category.Decaf))
        {
            product.WeightOptions = new List<WeightOption>
            {
                new() { Weight = "12 oz", PriceAdjustment = 0m, IsDefault = true },
                new() { Weight = "1 lb", PriceAdjustment = 4.00m },
                new() { Weight = "2 lb", PriceAdjustment = 10.00m },
                new() { Weight = "5 lb", PriceAdjustment = 28.00m }
            };
        }

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();
        return products;
    }

    private async Task SeedReviewsAsync(List<Product> products, Dictionary<string, ApplicationUser> users)
    {
        var reviews = new List<Review>();
        var random = new Random(42); // Fixed seed for reproducibility
        var reviewTexts = new[]
        {
            ("Amazing flavor!", "This is hands down one of the best coffees I've ever had. The flavor notes are spot on and the roast is perfect."),
            ("Great daily brew", "I've been drinking this every morning for a month now. Consistently good and great value for the quality."),
            ("Exceeded expectations", "I wasn't sure what to expect but this coffee blew me away. Will definitely order again."),
            ("Good but not great", "Solid coffee with nice flavor, but I've had better at this price point. Still would recommend trying it."),
            ("Perfect for espresso", "I use this in my espresso machine and it pulls beautiful shots. Creamy and rich with excellent crema."),
            ("Love the aroma", "The smell when you open the bag is incredible. Tastes as good as it smells. My new favorite."),
            ("Smooth and balanced", "Really well-balanced coffee. Not too acidic, not too bitter. Just right for my taste."),
            ("Disappointing", "Expected more based on the description. The flavor was flat and lacked the complexity I was hoping for."),
            ("Best gift ever", "Bought this as a gift for my dad and he loved it so much I had to order some for myself!"),
            ("Rich and complex", "Every sip reveals new flavors. This is what specialty coffee should taste like."),
        };

        var coffeeProducts = products.Where(p => p.Category is Category.SingleOrigin or Category.Blend or Category.Decaf).ToList();
        var userKeys = new[] { "sarah", "mike" };

        foreach (var product in coffeeProducts)
        {
            var reviewCount = random.Next(2, 5);
            for (int i = 0; i < reviewCount && i < reviewTexts.Length; i++)
            {
                var userKey = userKeys[i % userKeys.Length];
                var user = users[userKey];
                var (title, body) = reviewTexts[(product.Id + i) % reviewTexts.Length];
                var rating = random.Next(3, 6); // 3-5 stars mostly

                if (!reviews.Any(r => r.UserId == user.Id && r.ProductId == product.Id))
                {
                    reviews.Add(new Review
                    {
                        ProductId = product.Id,
                        UserId = user.Id,
                        UserName = $"{user.FirstName} {user.LastName}",
                        Rating = rating,
                        Title = title,
                        Body = body,
                        IsVerifiedPurchase = random.Next(100) < 70,
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 90))
                    });
                }
            }
        }

        _context.Reviews.AddRange(reviews);
        await _context.SaveChangesAsync();
    }

    private async Task SeedOrdersAsync(List<Product> products, Dictionary<string, ApplicationUser> users)
    {
        var random = new Random(42);
        var orders = new List<Order>();
        var coffeeProducts = products.Where(p => p.Category is Category.SingleOrigin or Category.Blend).Take(6).ToList();

        var statuses = new[] { OrderStatus.Delivered, OrderStatus.Delivered, OrderStatus.Shipped, OrderStatus.Processing, OrderStatus.Pending };

        foreach (var (userKey, index) in new[] { ("sarah", 0), ("mike", 1) })
        {
            var user = users[userKey];
            for (int i = 0; i < 5; i++)
            {
                var orderProducts = coffeeProducts.Skip(random.Next(0, 3)).Take(random.Next(1, 3)).ToList();
                var items = orderProducts.Select(p => new OrderItem
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductSlug = p.Slug,
                    UnitPrice = p.Price,
                    Quantity = random.Next(1, 3),
                    Weight = "12 oz",
                    Grind = "Whole Bean",
                    LineTotal = p.Price * random.Next(1, 3)
                }).ToList();

                var subtotal = items.Sum(it => it.LineTotal);
                var shipping = subtotal >= 50m ? 0m : 5.99m;
                var tax = Math.Round(subtotal * 0.08m, 2);

                orders.Add(new Order
                {
                    OrderNumber = $"SR-2025{(1 + index * 5 + i):D4}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                    UserId = user.Id,
                    ShippingName = $"{user.FirstName} {user.LastName}",
                    ShippingAddressLine1 = userKey == "sarah" ? "123 Coffee Lane" : "456 Roast Ave",
                    ShippingCity = userKey == "sarah" ? "Portland" : "Seattle",
                    ShippingState = userKey == "sarah" ? "OR" : "WA",
                    ShippingZipCode = userKey == "sarah" ? "97201" : "98101",
                    ShippingCountry = "US",
                    Subtotal = subtotal,
                    ShippingCost = shipping,
                    Tax = tax,
                    Total = subtotal + shipping + tax,
                    Status = statuses[i],
                    PaymentStatus = PaymentStatus.Paid,
                    Items = items,
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 60)),
                    UpdatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30))
                });
            }
        }

        _context.Orders.AddRange(orders);
        await _context.SaveChangesAsync();
    }

    private async Task SeedDiscountCodesAsync()
    {
        var codes = new List<DiscountCode>
        {
            new() { Code = "WELCOME10", Type = DiscountType.Percentage, Value = 10m, IsActive = true, ExpiresAt = DateTime.UtcNow.AddYears(1) },
            new() { Code = "FIVEBUCKS", Type = DiscountType.FixedAmount, Value = 5m, MinimumOrderAmount = 30m, IsActive = true, ExpiresAt = DateTime.UtcNow.AddMonths(6) },
            new() { Code = "FREESHIP", Type = DiscountType.FreeShipping, Value = 0m, IsActive = true, ExpiresAt = DateTime.UtcNow.AddMonths(3) }
        };

        _context.DiscountCodes.AddRange(codes);
        await _context.SaveChangesAsync();
    }
}
