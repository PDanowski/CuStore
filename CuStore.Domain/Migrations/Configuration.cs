using CuStore.Domain.Concrete;
using CuStore.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CuStore.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CuStore.Domain.Concrete.StoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "CuStore.Domain.Concrete.StoreContext";
        }

        protected override void Seed(StoreContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            SeedRoles(context);
            SeedUsers(context);
            SeedCategories(context);
            SeedShippingMethods(context);
            SeedProducts(context);
        }

        private void SeedRoles(StoreContext context)
        {
            var rolesManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

            if (!rolesManager.RoleExists("Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };
                rolesManager.Create(role);
            }

            if (!rolesManager.RoleExists("User"))
            {
                var role = new IdentityRole { Name = "User" };
                rolesManager.Create(role);
            }

            context.SaveChanges();
        }

        private void SeedUsers(StoreContext context)
        {
            var store = new UserStore<User>(context);
            var manager = new UserManager<User>(store);

            if (!context.Users.Any(u => u.UserName.Equals("admin")))
            {
                var user = new User
                {
                    UserName = "admin@custore.com",
                    BirthDate = DateTime.Today,
                    Email = "admin@custore.com",
                    FirstName = "admin",
                    LastName = "@"
                };
                var adminResult = manager.Create(user, "Admin123+");

                if (adminResult.Succeeded)
                {
                    manager.AddToRole(user.Id, "Admin");
                }
            }

            if (!context.Users.Any(u => u.UserName.Equals("user")))
            {
                var user = new User
                {
                    UserName = "user@custore.com",
                    BirthDate = DateTime.Today,
                    Email = "user@custore.com",
                    FirstName = "user",
                    LastName = "@"
                };
                var adminResult = manager.Create(user, "User123+");

                if (adminResult.Succeeded)
                {
                    manager.AddToRole(user.Id, "User");
                }
            }
        }

        private void SeedCategories(StoreContext context)
        {
            var category1 =
                new Category
                {
                    Id = 1,
                    Name = "Smartphones"
                };
            context.Set<Category>().AddOrUpdate(category1);

            var category2 =
                new Category
                {
                    Id = 2,
                    ParentCategoryId = 1,
                    Name = "Samsung"
                };
            context.Set<Category>().AddOrUpdate(category2);

            var category3 =
                new Category
                {
                    Id = 3,
                    ParentCategoryId = 1,
                    Name = "Apple"
                };
            context.Set<Category>().AddOrUpdate(category3);

            var category4 =
                new Category
                {
                    Id = 4,
                    ParentCategoryId = 1,
                    Name = "Huawei"
                };
            context.Set<Category>().AddOrUpdate(category4);

            var category5 =
                new Category
                {
                    Id = 5,
                    ParentCategoryId = 1,
                    Name = "Sony"
                };
            context.Set<Category>().AddOrUpdate(category5);

            var category6 =
                new Category
                {
                    Id = 6,
                    ParentCategoryId = 1,
                    Name = "Xiaomi"
                };
            context.Set<Category>().AddOrUpdate(category6);

            var category7 =
                new Category
                {
                    Id = 7,
                    ParentCategoryId = 1,
                    Name = "OnePlus"
                };
            context.Set<Category>().AddOrUpdate(category7);

            var category8 =
                new Category
                {
                    Id = 8,
                    Name = "Tablets"
                };
            context.Set<Category>().AddOrUpdate(category8);

            var category9 =
                new Category
                {
                    Id = 9,
                    ParentCategoryId = 8,
                    Name = "Apple"
                };
            context.Set<Category>().AddOrUpdate(category9);

            var category10 =
                new Category
                {
                    Id = 10,
                    ParentCategoryId = 8,
                    Name = "Samsung"
                };
            context.Set<Category>().AddOrUpdate(category10);

            var category11 =
                new Category
                {
                    Id = 11,
                    ParentCategoryId = 8,
                    Name = "Huawei"
                };
            context.Set<Category>().AddOrUpdate(category11);

            var category12 =
                new Category
                {
                    Id = 12,
                    ParentCategoryId = 8,
                    Name = "Lenovo"
                };
            context.Set<Category>().AddOrUpdate(category12);

            context.SaveChanges();
        }

        private void SeedShippingMethods(StoreContext context)
        {
            var shippingMethod1 =
                new ShippingMethod
                {
                    Id = 1,
                    Name = "DHL Parcel",
                    MaximumPcs = 100,
                    Price = 16.00M
                };
            context.Set<ShippingMethod>().AddOrUpdate(shippingMethod1);

            var shippingMethod2 =
                new ShippingMethod
                {
                    Id = 2,
                    Name = "InPost 48",
                    MaximumPcs = 3,
                    Price = 9.00M
                };
            context.Set<ShippingMethod>().AddOrUpdate(shippingMethod2);

            var shippingMethod3 =
                new ShippingMethod
                {
                    Id = 3,
                    Name = "Post office",
                    MaximumPcs = 5,
                    Price = 12.00M
                };
            context.Set<ShippingMethod>().AddOrUpdate(shippingMethod3);

            context.SaveChanges();
        }

        private void SeedProducts(StoreContext context)
        {
            var product1 =
                new Product
                {
                    Id = 1,
                    Name = "Samsung Galaxy A8",
                    CategoryId = 2,
                    Price = 1599.00M,
                    QuanityInStock = 10
                };
            context.Set<Product>().AddOrUpdate(product1);

            var product2 =
                new Product
                {
                    Id = 2,
                    Name = "Samsung Galaxy S9",
                    CategoryId = 2,
                    Price = 2299.00M,
                    QuanityInStock = 35
                };
            context.Set<Product>().AddOrUpdate(product2);

            var product3 =
                new Product
                {
                    Id = 3,
                    Name = "Samsung Galaxy S9+",
                    CategoryId = 2,
                    Price = 2799.00M,
                    QuanityInStock = 15
                };
            context.Set<Product>().AddOrUpdate(product3);

            var product4 =
                new Product
                {
                    Id = 4,
                    Name = "Apple iPhone X",
                    CategoryId = 3,
                    Price = 4599.00M,
                    QuanityInStock = 30
                };
            context.Set<Product>().AddOrUpdate(product4);

            var product5 =
                new Product
                {
                    Id = 5,
                    Name = "Apple iPhone Xs",
                    CategoryId = 3,
                    Price = 5399.00M,
                    QuanityInStock = 10
                };
            context.Set<Product>().AddOrUpdate(product5);

            var product6 =
                new Product
                {
                    Id = 6,
                    Name = "Huawei P20",
                    CategoryId = 4,
                    Price = 1999.00M,
                    QuanityInStock = 20
                };
            context.Set<Product>().AddOrUpdate(product6);

            var product7 =
                new Product
                {
                    Id = 7,
                    Name = "Huawei P20 Pro",
                    CategoryId = 4,
                    Price = 2699.00M,
                    QuanityInStock = 10
                };
            context.Set<Product>().AddOrUpdate(product7);

            var product8 =
                new Product
                {
                    Id = 8,
                    Name = "Huawei Mate 20",
                    CategoryId = 4,
                    Price = 2899.00M,
                    QuanityInStock = 50
                };
            context.Set<Product>().AddOrUpdate(product8);

            var product9 =
                new Product
                {
                    Id = 9,
                    Name = "Huawei Mate 20 Pro",
                    CategoryId = 4,
                    Price = 4199.00M,
                    QuanityInStock = 20
                };
            context.Set<Product>().AddOrUpdate(product9);

            var product10 =
                new Product
                {
                    Id = 10,
                    Name = "Huawei Mate 20 Lite",
                    CategoryId = 4,
                    Price = 1199.00M,
                    QuanityInStock = 100
                };
            context.Set<Product>().AddOrUpdate(product10);

            var product11 =
                new Product
                {
                    Id = 11,
                    Name = "Sony Xperia XZ2",
                    CategoryId = 5,
                    Price = 2050.00M,
                    QuanityInStock = 15
                };
            context.Set<Product>().AddOrUpdate(product11);

            var product12 =
                new Product
                {
                    Id = 12,
                    Name = "Sony Xperia XZ3",
                    CategoryId = 5,
                    Price = 2750.00M,
                    QuanityInStock = 25
                };
            context.Set<Product>().AddOrUpdate(product12);

            var product13 =
                new Product
                {
                    Id = 13,
                    Name = "Xiaomi Mi8",
                    CategoryId = 6,
                    Price = 1990.00M,
                    QuanityInStock = 45
                };
            context.Set<Product>().AddOrUpdate(product13);

            var product14 =
                new Product
                {
                    Id = 14,
                    Name = "Xiaomi Mi8 Lite",
                    CategoryId = 6,
                    Price = 1290.00M,
                    QuanityInStock = 30
                };
            context.Set<Product>().AddOrUpdate(product14);

            var product15 =
                new Product
                {
                    Id = 15,
                    Name = "Xiaomi Mi Mix 3",
                    CategoryId = 6,
                    Price = 1790.00M,
                    QuanityInStock = 30
                };
            context.Set<Product>().AddOrUpdate(product15);

            var product16 =
                new Product
                {
                    Id = 16,
                    Name = "OnePlus 6",
                    CategoryId = 7,
                    Price = 1999.00M,
                    QuanityInStock = 20
                };
            context.Set<Product>().AddOrUpdate(product16);

            var product17 =
                new Product
                {
                    Id = 17,
                    Name = "OnePlus 6T",
                    CategoryId = 7,
                    Price = 2499.00M,
                    QuanityInStock = 50
                };
            context.Set<Product>().AddOrUpdate(product17);

            var product18 =
                new Product
                {
                    Id = 18,
                    Name = "Apple iPad 32GB",
                    CategoryId = 9,
                    Price = 1399.00M,
                    QuanityInStock = 10
                };
            context.Set<Product>().AddOrUpdate(product18);

            var product19 =
                new Product
                {
                    Id = 19,
                    Name = "Apple iPad 64GB",
                    CategoryId = 9,
                    Price = 1529.00M,
                    QuanityInStock = 10
                };
            context.Set<Product>().AddOrUpdate(product19);


            var product20 =
                new Product
                {
                    Id = 20,
                    Name = "Samsung Galaxy Tab A 10.1",
                    CategoryId = 10,
                    Price = 799.00M,
                    QuanityInStock = 10
                };
            context.Set<Product>().AddOrUpdate(product20);

            var product21 =
                new Product
                {
                    Id = 21,
                    Name = "Samsung Galaxy Tab S4 10.5",
                    CategoryId = 10,
                    Price = 2799.00M,
                    QuanityInStock = 10
                };
            context.Set<Product>().AddOrUpdate(product21);

            var product22 =
                new Product
                {
                    Id = 22,
                    Name = "Huawei MediaPad M3 10",
                    CategoryId = 11,
                    Price = 999.00M,
                    QuanityInStock = 60
                };
            context.Set<Product>().AddOrUpdate(product22);

            var product23 =
                new Product
                {
                    Id = 23,
                    Name = "Huawei MediaPad M5 10",
                    CategoryId = 11,
                    Price = 1299.00M,
                    QuanityInStock = 100
                };
            context.Set<Product>().AddOrUpdate(product23);

            var product24 =
                new Product
                {
                    Id = 24,
                    Name = "Huawei MediaPad T3 10",
                    CategoryId = 11,
                    Price = 499.00M,
                    QuanityInStock = 50
                };
            context.Set<Product>().AddOrUpdate(product24);

            var product25 =
                new Product
                {
                    Id = 25,
                    Name = "Lenovo TAB 4 10",
                    CategoryId = 11,
                    Price = 510.00M,
                    QuanityInStock = 10
                };
            context.Set<Product>().AddOrUpdate(product25);

            context.SaveChanges();
        }
    }
}
