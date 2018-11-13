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
            Database.SetInitializer(new CreateDatabaseIfNotExists<StoreContext>());
            Debug.Print(Categories.Count().ToString());
            //this.Categories.Add(new Category {Name = "test"});
            //this.SaveChanges();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Order>()
                .HasRequired(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Category>()
                .HasOptional(c => c.ParentCategory)
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId);
        }
    }
}
