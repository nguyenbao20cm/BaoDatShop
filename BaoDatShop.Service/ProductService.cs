using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.ProductType;
using BaoDatShop.Responsitories;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IProductService
    {
        public bool Create(CreateProduct model);
        public bool Update(int id,CreateProduct model);
        public bool Delete(int id);
        public Product GetById(int id);
        public List<Product> GetAll();
    }
    public class ProductService : IProductService
    {
        private readonly IProductResponsitories productResponsitories;
        public ProductService(IProductResponsitories productResponsitories)
        {
            this.productResponsitories = productResponsitories;
        }
    
        public bool Create(CreateProduct model)
        {
            Product result = new();
            result.SKU=model.SKU;
            result.Name = model.Name; 
            result.Description = model.Description;
            result.Price = model.Price;
            result.Stock = model.Stock;
            result.ProductTypeId = model.ProductTypeId;
            result.Image = model.Image;
            result.Status = true;
            return productResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            Product result = productResponsitories.GetById(id);
            result.Status = false;
            return productResponsitories.Update(result);
        }

        public List<Product> GetAll()
        {
            return productResponsitories.GetAll();
        }

        public Product GetById(int id)
        {
            return productResponsitories.GetById(id);
        }

        public bool Update(int id,CreateProduct model)
        {
            Product result = productResponsitories.GetById(id);
            result.SKU = model.SKU;
            result.Name = model.Name;
            result.Description = model.Description;
            result.Price = model.Price;
            result.Stock = model.Stock;
            result.ProductTypeId = model.ProductTypeId;
            result.Image = model.Image;
            return productResponsitories.Update(result);
        }
    }
}
