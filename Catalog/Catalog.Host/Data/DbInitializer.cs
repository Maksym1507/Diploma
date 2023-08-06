using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.CatalogTypes.Any())
            {
                await context.CatalogTypes.AddRangeAsync(GetPreconfiguredCatalogTypes());

                await context.SaveChangesAsync();
            }

            if (!context.CatalogItems.Any())
            {
                await context.CatalogItems.AddRangeAsync(GetPreconfiguredItems());

                await context.SaveChangesAsync();
            }
        }

        private static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
        {
            return new List<CatalogType>()
            {
                new CatalogType() { Type = "pizza" },
                new CatalogType() { Type = "roll" },
                new CatalogType() { Type = "shawarma" },
                new CatalogType() { Type = "burger" },
                new CatalogType() { Type = "sushi" }
            };
        }

        private static IEnumerable<CatalogItem> GetPreconfiguredItems()
        {
            return new List<CatalogItem>()
            {
                new CatalogItem
                {
                    CatalogTypeId = 3,
                    Description = "chicken thigh, fresh cabbage, pickled cucumber, tomato, carrot, Mars onion, potato dips, shawarma sauce in lavash",
                    Title = "Max Deep",
                    Price = 13.7M,
                    Weight = 450,
                    PictureFileName = "1.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 1,
                    Description = "mozzarella, mushrooms, salami, pesto sauce, tomato sauce, oregano",
                    Title = "Saint Diablo",
                    Price = 15.3M,
                    Weight = 870,
                    PictureFileName = "2.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 4,
                    Description = "beef cutlet, pickled cucumber, onion, ketchup, American mustard, butter",
                    Title = "Hamburger",
                    Price = 16.1M,
                    Weight = 120,
                    PictureFileName = "3.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 1,
                    Description = "pepperoni, hunting sausages, feta, olives, sriracha, mozzarella, oregano, tomato sauce",
                    Title = "Pepperoni",
                    Price = 12.7M,
                    Weight = 800,
                    PictureFileName = "4.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 5,
                    Description = "sushi with salmon",
                    Title = "Sushi with salmon",
                    Price = 10.5M,
                    Weight = 35,
                    PictureFileName = "5.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 2,
                    Description = "smoked chicken, cream cheese, pineapple, mayonnaise, cheddar cheese, unagi sauce",
                    Title = "Hawaiian",
                    Price = 5.7M,
                    Weight = 290,
                    PictureFileName = "6.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 2,
                    Description = "smoked salmon, chuka, cucumber, bell pepper, iceberg lettuce, cheddar cheese, spice sauce, french onion",
                    Title = "Spring Fish",
                    Price = 13.7M,
                    Weight = 450,
                    PictureFileName = "7.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 1,
                    Description = "Feta, mozzarella, broccoli, olives, olives, bell peppers, tomatoes, mars onion, oregano, tomato sauce",
                    Title = "Vegetarian",
                    Price = 9.3M,
                    Weight = 550,
                    PictureFileName = "8.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 2,
                    Description = "cocktail shrimp, salmon, takuan, pumpkin, cream cheese, batter, panko crackers, spice sauce, unagi sauce, peanuts",
                    Title = "Unagi Crunch",
                    Price = 15.4M,
                    Weight = 300,
                    PictureFileName = "9.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 1,
                    Description = "mozzarella, ham, olives, tomatoes, parsley, mushrooms, oregano, tomato sauce",
                    Title = "Pizza with ham and mushrooms",
                    Price = 15.4M,
                    Weight = 900,
                    PictureFileName = "10.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 2,
                    Description = "salmon, shrimps, cream cheese, cucumber, spice sauce, batter, panko crackers",
                    Title = "Shrimp Balls",
                    Price = 5.1M,
                    Weight = 240,
                    PictureFileName = "11.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 1,
                    Description = "mozzarella, hunting sausages, bacon, boiled pork, blue onion, tomato sauce, parsley, oregano",
                    Title = "Pizza Boom of meat",
                    Price = 12.1M,
                    Weight = 850,
                    PictureFileName = "12.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 2,
                    Description = "pumpkin, takuan, chuka, bell pepper, cream cheese",
                    Title = "Vegetus",
                    Price = 7.7M,
                    Weight = 220,
                    PictureFileName = "13.png",
                    AvailableStock = 100
                },
                new CatalogItem
                {
                    CatalogTypeId = 1,
                    Description = "mozzarella, salmon, mussels, squid, tomato, lemon, pesto, oregano, tomato sauce",
                    Title = "Seafood",
                    Price = 14.6M,
                    Weight = 550,
                    PictureFileName = "14.png",
                    AvailableStock = 100
                },
            };
        }
    }
}
