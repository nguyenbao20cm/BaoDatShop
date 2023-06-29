using BaoDatShop.DTO.ProductSize;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Eshop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface IProductSizeResponsitories
    {
        public bool Create(ProductSize model);
        public bool Update(ProductSize model);
        public ProductSize GetById(int id);
        public List<ProductSize> GetAll();


    }
    public class ProductSizeResponsitories: IProductSizeResponsitories
    {
        private readonly AppDbContext context;
        public ProductSizeResponsitories(AppDbContext context)
        {
            this.context = context;
        }
     
        public bool Create(ProductSize model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<ProductSize> GetAll()
        {
            if (context.ProductSize.ToList() == null) return null;
            return context.ProductSize.Include(a => a.Supplier).Include(a=>a.Product).ToList();
        }



        public ProductSize GetById(int id)
        {
            if (context.ProductSize.Where(a => a.Id == id).FirstOrDefault() == null) return null;
            return context.ProductSize.Where(a => a.Id == id).FirstOrDefault();
        }

        public bool Update(ProductSize model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
