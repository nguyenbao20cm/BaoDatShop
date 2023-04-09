using BaoDatShop.Model.Context;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface IProductTypeResponsitories
    {
        public bool Create(ProductTypes model);
        public bool Update(ProductTypes model);
        public ProductTypes GetById(int id);
        public List<ProductTypes> GetAll();
    }
    public class ProductTypeResponsitories: IProductTypeResponsitories
    {
        private readonly AppDbContext context;
        public ProductTypeResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(ProductTypes model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<ProductTypes> GetAll()
        {
            if (context.ProductType.Where(a=>a.Status==true).ToList() == null) return null;
            return context.ProductType.Where(a => a.Status == true).ToList();
        }

        public ProductTypes GetById(int id)
        {
            if (context.ProductType.Where(a => a.Id == id).Where(a => a.Status == true).FirstOrDefault() == null) return null;
            return context.ProductType.Where(a => a.Id == id).FirstOrDefault();
        }

        public bool Update(ProductTypes model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
