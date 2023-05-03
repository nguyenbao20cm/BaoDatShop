﻿using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Eshop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface IProductResponsitories
    {
        public bool Create(Product model);
        public bool Update(Product model);
        public Product GetById(int id);
        public List<Product> GetAll();
    }
    public class ProductResponsitories: IProductResponsitories
    {
        private readonly AppDbContext context;
        public ProductResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(Product model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<Product> GetAll()
        {
            if (context.Product.Where(a => a.Status == true).ToList() == null) return null;
            return context.Product.Where(a => a.Status == true).ToList();
        }

        public Product GetById(int id)
        {
            if (context.Product.Where(a => a.Id == id).Where(a => a.Status == true).FirstOrDefault() == null) return null;
            return context.Product.Where(a => a.Id == id).Where(a => a.Status == true).FirstOrDefault();
        }

        public bool Update(Product model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
