using BaoDatShop.DTO;
using BaoDatShop.Model.Model;
using Eshop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BaoDatShop.Model.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Account> Account { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetail { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductTypes> ProductType { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<NewDetail> NewDetail { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<AdvertisingPanel> AdvertisingPanel { get; set; }
        public DbSet<Disscount> Disscount { get; set; }
        public DbSet<ProductSize> ProductSize { get; set; }
        public DbSet<ImageProduct> ImageProduct { get; set; }
        public DbSet<EmailCustomer> EmailCustomer { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<News>(builder =>
            {
                builder.ToTable(nameof(News));
                builder.HasKey(x => x.NewsId);
            });

            modelBuilder.Entity<Product>()
             .HasIndex(p => new { p.Name,p.SKU }).IsUnique();
            modelBuilder.Entity<Disscount>()
             .HasIndex(p => new { p.ProductId }).IsUnique();

            modelBuilder.Entity<ProductTypes>()
           .HasIndex(p => new { p.Name }).IsUnique();
            modelBuilder.Entity<NewDetail>(builder =>
            {
                builder.ToTable(nameof(NewDetail));
                builder.HasKey(x => x.NewDetailId);
                builder
                .HasOne(x => x.New)
                .WithMany(x => x.NewDetail)
                .HasForeignKey(x => x.NewId);
            });
            modelBuilder.Entity<Cart>(builder =>
            {
                builder.ToTable(nameof(Cart));
                builder.HasKey(x => x.Id);
                builder
                .HasOne(x => x.ProductSize)
                .WithMany(x => x.Cart)
                .HasForeignKey(x => x.ProductSizeId);
            });
        }
    }
}
