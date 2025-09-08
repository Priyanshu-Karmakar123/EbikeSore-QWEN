using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EbikeStore.Data;
using EbikeStore.Models;

namespace EbikeStore.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Create roles
                await CreateRoles(roleManager);

                // Create admin user
                await CreateAdminUser(userManager);

                // Create staff user
                await CreateStaffUser(userManager);

                // Create categories
                await CreateCategories(context);

                // Create brands
                await CreateBrands(context);

                // Create products
                await CreateProducts(context);

                // Ensure all changes are saved
                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Staff", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task CreateAdminUser(UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "admin@example.com";
            const string adminPassword = "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true  // As per instructions
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private static async Task CreateStaffUser(UserManager<ApplicationUser> userManager)
        {
            const string staffEmail = "staff@example.com";
            const string staffPassword = "Staff123!";

            if (await userManager.FindByEmailAsync(staffEmail) == null)
            {
                var staffUser = new ApplicationUser
                {
                    UserName = staffEmail,
                    Email = staffEmail,
                    FirstName = "Staff",
                    LastName = "User",
                    EmailConfirmed = true  // As per instructions
                };

                var result = await userManager.CreateAsync(staffUser, staffPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(staffUser, "Staff");
                }
            }
        }

        private static async Task CreateCategories(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new[]
                {
                    new Category { Name = "Mountain", Description = "Electric bikes designed for off-road cycling" },
                    new Category { Name = "City", Description = "Electric bikes designed for urban commuting" },
                    new Category { Name = "Folding", Description = "Compact electric bikes that can be folded for easy storage" },
                    new Category { Name = "Cargo", Description = "Electric bikes designed to carry heavy loads" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateBrands(ApplicationDbContext context)
        {
            if (!context.Brands.Any())
            {
                var brands = new[]
                {
                    new Brand { Name = "Eskuta", LogoUrl = "/images/placeholder.svg" },
                    new Brand { Name = "Rocket", LogoUrl = "/images/placeholder.svg" },
                    new Brand { Name = "Claud Butler", LogoUrl = "/images/placeholder.svg" },
                    new Brand { Name = "Emovement", LogoUrl = "/images/placeholder.svg" }
                };

                context.Brands.AddRange(brands);
                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateProducts(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                // Get categories and brands for foreign key references
                var mountainCategory = await context.Categories.FirstAsync(c => c.Name == "Mountain");
                var cityCategory = await context.Categories.FirstAsync(c => c.Name == "City");
                var foldingCategory = await context.Categories.FirstAsync(c => c.Name == "Folding");
                var cargoCategory = await context.Categories.FirstAsync(c => c.Name == "Cargo");

                var eskutaBrand = await context.Brands.FirstAsync(b => b.Name == "Eskuta");
                var rocketBrand = await context.Brands.FirstAsync(b => b.Name == "Rocket");
                var claudBrand = await context.Brands.FirstAsync(b => b.Name == "Claud Butler");
                var emovementBrand = await context.Brands.FirstAsync(b => b.Name == "Emovement");

                var products = new[]
                {
                    new Product
                    {
                        Name = "Eskuta SX250 Series 4 Classic Moped",
                        Description = "Classic design with modern electric power. Perfect for city commuting with its comfortable riding position and reliable motor.",
                        Price = 1299.99m,
                        Sku = "ESK-SX250-001",
                        StockQuantity = 15,
                        ImageUrl = "/images/bike-eskuta-sx250-series4-classic-moped-black.png",
                        CategoryId = cityCategory.Id,
                        BrandId = eskutaBrand.Id
                    },
                    new Product
                    {
                        Name = "Eskuta SX250 Voyager Max Electric",
                        Description = "Extended range electric bike with premium components. Ideal for longer commutes and weekend adventures.",
                        Price = 1599.99m,
                        Sku = "ESK-SX250-VMAX",
                        StockQuantity = 10,
                        ImageUrl = "/images/bike-eskuta-sx250-voyager-max-electric-white.png",
                        CategoryId = cityCategory.Id,
                        BrandId = eskutaBrand.Id
                    },
                    new Product
                    {
                        Name = "Rocket ST Fattyre Electric Bike",
                        Description = "Fat tire electric bike designed for all terrains. Excellent for beach rides, snow, and rough trails.",
                        Price = 1899.99m,
                        Sku = "RKT-ST-FATTY",
                        StockQuantity = 8,
                        ImageUrl = "/images/rocket-st-fattyre-electric-bike-side.png",
                        CategoryId = mountainCategory.Id,
                        BrandId = rocketBrand.Id
                    },
                    new Product
                    {
                        Name = "Rocket SX Sport Utility Electric Bike 250W",
                        Description = "Versatile electric bike with sporty design and practical features. Great for both city commuting and light trail riding.",
                        Price = 1399.99m,
                        Sku = "RKT-SX-250W",
                        StockQuantity = 12,
                        ImageUrl = "/images/rocket-st-fattyre-electric-bike-side.png",
                        CategoryId = cityCategory.Id,
                        BrandId = rocketBrand.Id
                    },
                    new Product
                    {
                        Name = "Claud Butler Wrath2 Cues",
                        Description = "Premium folding electric bike with lightweight frame. Perfect for commuters who need to store their bike in small spaces.",
                        Price = 2199.99m,
                        Sku = "CLD-WRATH2-CUES",
                        StockQuantity = 6,
                        ImageUrl = "/images/claud-butler-wrath2-cues-angled.png",
                        CategoryId = foldingCategory.Id,
                        BrandId = claudBrand.Id
                    },
                    new Product
                    {
                        Name = "Emovement Raven Electric Bike",
                        Description = "Powerful cargo electric bike designed to carry heavy loads. Ideal for families or businesses needing to transport goods.",
                        Price = 2799.99m,
                        Sku = "EMV-RVN-001",
                        StockQuantity = 5,
                        ImageUrl = "/images/emovement-raven-electric-bike-side.png",
                        CategoryId = cargoCategory.Id,
                        BrandId = emovementBrand.Id
                    }
                };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();

                // Add specifications for each product
                foreach (var product in products)
                {
                    var specifications = new[]
                    {
                        new ProductSpecification { Key = "Motor", Value = "250W Rear Hub Motor", ProductId = product.Id },
                        new ProductSpecification { Key = "Battery", Value = "36V 10.4Ah Lithium-Ion", ProductId = product.Id },
                        new ProductSpecification { Key = "Range", Value = "Up to 50 miles", ProductId = product.Id },
                        new ProductSpecification { Key = "Charging Time", Value = "6-8 hours", ProductId = product.Id },
                        new ProductSpecification { Key = "Frame", Value = "Aluminum Alloy", ProductId = product.Id },
                        new ProductSpecification { Key = "Weight", Value = "25kg", ProductId = product.Id }
                    };

                    context.ProductSpecifications.AddRange(specifications);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}