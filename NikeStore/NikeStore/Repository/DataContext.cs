using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NikeStore.Models;

namespace NikeStore.Repository
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Models.Product> Product { get; set; }
        public DbSet<Models.ProductQuantity> ProductQuantity { get; set; }
        public DbSet<Models.Promotion> Promotion { get; set; }
        public DbSet<Models.ProductCategory> ProductCategory { get; set; }
        public DbSet<Models.ProductColor> ProductColor { get; set; }
        public DbSet<Models.ProductImage> ProductImage { get; set; }
        public DbSet<Models.ProductSize> ProductSize { get; set; }
        public DbSet<Models.ProductType> ProductType { get; set; }
        public DbSet<Models.ProductGender> ProductGender { get; set; }
        public DbSet<Models.Service> Service { get; set; }
        public DbSet<Models.ServiceType> ServiceType { get; set; }
        public DbSet<Models.Account> Account { get; set; }
        public DbSet<Models.Payment> Payment { get; set; }
        public DbSet<Models.PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Models.Provider> Provider { get; set; }
        public DbSet<Models.Warehouse> Warehouse { get; set; }
        public DbSet<Models.Importing> Importing { get; set; }
        public DbSet<Models.ImportingDetail> ImportingDetail { get; set; }
        public DbSet<Models.Order> Order { get; set; }
        public DbSet<Models.OrderDetail> OrderDetail { get; set; }
        public DbSet<Models.WishList> WishList { get; set; }
    }
}
