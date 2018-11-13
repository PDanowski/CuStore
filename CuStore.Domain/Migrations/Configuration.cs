using System;
using System.Data.Entity.Migrations;
using System.Linq;
using CuStore.Domain.Concrete;
using CuStore.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CuStore.Domain.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<StoreContext>
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

            SeedRoles();
            SeedUsers(context);
            SeedCategories(context);
        }

        private void SeedRoles()
        {
            var rolesManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

            if (!rolesManager.RoleExists("Admin"))
            {
                var role = new IdentityRole {Name = "Admin"};
                rolesManager.Create(role);
            }

            if (!rolesManager.RoleExists("User"))
            {
                var role = new IdentityRole {Name = "User"};
                rolesManager.Create(role);
            }
        }

        private void SeedUsers(StoreContext context)
        {
            var store = new UserStore<User>(context);
            var manager = new UserManager<User>(store);

            if (!context.Users.Any(u => u.UserName.Equals("admin")))
            {
                var user = new User { UserName = "admin@custore.com", BirthDate = DateTime.Today, Email = "admin@custore.com" };
                var adminResult = manager.Create(user, "Admin123+");

                if (adminResult.Succeeded)
                {
                    manager.AddToRole(user.Id, "Admin");
                }
            }

            if (!context.Users.Any(u => u.UserName.Equals("user")))
            {
                var user = new User { UserName = "user@custore.com", BirthDate = DateTime.Today, Email = "user@custore.com" };
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
    }
}
