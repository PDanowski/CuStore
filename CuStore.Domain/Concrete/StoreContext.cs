using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using Microsoft.AspNet.Identity;
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
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<StoreContext>(null);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Cart>()
                .HasOptional(x => x.User)
                .WithMany(x => x.Carts)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            // Configure Cart & Order entity
            modelBuilder.Entity<Cart>()
                .HasOptional(c => c.Order) // Mark Order property optional in Cart entity
                .WithRequired(o => o.Cart); // mark Cart property as required in Order entity. Cannot save Order without Cart

            // configures one-to-many relationship
            modelBuilder.Entity<CartItem>()
                .HasRequired(c => c.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(c => c.CartId);

            // configures one-to-many relationship
            modelBuilder.Entity<CartItem>()
                .HasRequired(c => c.Product)
                .WithMany(c => c.CartItems)
                .HasForeignKey(c => c.ProductId);

            // configures one-to-many relationship
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // configures one-to-many relationship
            modelBuilder.Entity<Order>()
                .HasRequired(o => o.ShippingMethod)
                .WithMany(sm => sm.Orders)
                .HasForeignKey(o => o.ShippingMethodId);

            // configures one-to-one-itself relationship
            modelBuilder.Entity<Category>()
                .HasOptional(c => c.ParentCategory)
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId);

            // Configure User & UserAddress entity
            modelBuilder.Entity<User>()
                .HasOptional(u => u.UserAddress) // Mark UserAddress property optional in User entity
                .WithRequired(ua => ua.User); // mark User property as required in UserAddress entity. Cannot save UserAddress without User

            // Configure Order & ShippingAddress entity
            modelBuilder.Entity<Order>()
                .HasOptional(o => o.ShippingAddress) // Mark ShippingAddress property optional in Order entity
                .WithRequired(sa => sa.Order); // mark Order property as required in ShippingAddress entity. Cannot save ShippingAddress without User

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Code)
                .IsUnique();
        }
    }
}
