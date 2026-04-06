using CuStore.Core.Entities;

namespace CuStore.Infrastructure.Data;

public static class StoreDbSeed
{
    public static void Seed(StoreDbContext db)
    {
        if (!db.Categories.Any())
        {
            var electronics = new Category { Name = "Electronics" };
            var books = new Category { Name = "Books" };
            db.Categories.AddRange(electronics, books);
            db.SaveChanges();

            db.Products.AddRange(
                new Product { Name = "Laptop", Description = "Development laptop", Code = "LAP-001", Price = 4500, QuanityInStock = 8, CategoryId = electronics.Id },
                new Product { Name = "Headphones", Description = "Noise cancelling", Code = "AUD-002", Price = 699, QuanityInStock = 20, CategoryId = electronics.Id },
                new Product { Name = "Domain-Driven Design", Description = "Architecture book", Code = "BOO-003", Price = 199, QuanityInStock = 50, CategoryId = books.Id }
            );
        }

        if (!db.ShippingMethods.Any())
        {
            db.ShippingMethods.AddRange(
                new ShippingMethod { Name = "Courier", Price = 20, MaximumPcs = 200 },
                new ShippingMethod { Name = "Parcel locker", Price = 12, MaximumPcs = 80 }
            );
        }

        if (!db.Users.Any())
        {
            db.Users.Add(new User
            {
                Id = "demo-user",
                UserName = "demo@custore.local",
                Email = "demo@custore.local",
                FirstName = "Demo",
                LastName = "User",
                BirthDate = new DateTime(1990, 1, 1),
            });
        }

        db.SaveChanges();
    }
}
