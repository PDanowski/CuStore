using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CuStore.Domain.Concrete
{
    public class StoreContext : IdentityDbContext, IStoreContext
    {
        public StoreContext() : base("name=CuStore")
        {
            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Launch();

            Database.SetInitializer(new CreateDatabaseIfNotExists<StoreContext>());
            //Debug.Print(Categories.Count().ToString());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Cart>()
                .HasOptional(x => x.User)
                .WithMany(x => x.Carts)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cart>()
                .HasOptional(x => x.Order)
                .WithRequired(x => x.Cart);

            modelBuilder.Entity<CartItem>()
                .HasRequired(c => c.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(c => c.CartId);

            modelBuilder.Entity<CartItem>()
                .HasRequired(c => c.Product)
                .WithMany(c => c.CartItems)
                .HasForeignKey(c => c.ProductId);

            modelBuilder.Entity<Order>()
                .HasRequired(c => c.ShippingMethod);

            modelBuilder.Entity<Category>()
                .HasOptional(c => c.ParentCategory)
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId);

            modelBuilder.Entity<User>()
                .HasOptional(c => c.Address)
                .WithMany()
                .HasForeignKey(c => c.AddressId);

        }
    }
}
