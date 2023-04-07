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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<News>(builder =>
            {
                builder.ToTable(nameof(News));
                builder.HasKey(x => x.NewsId);
            });
            modelBuilder.Entity<NewDetail>(builder =>
            {
                builder.ToTable(nameof(NewDetail));
                builder.HasKey(x => x.NewDetailId);
                builder
                .HasOne(x => x.New)
                .WithMany(x => x.NewDetail)
                .HasForeignKey(x => x.NewId);
            });
        }
    }
}
