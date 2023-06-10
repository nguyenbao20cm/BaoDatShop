using BaoDatShop.DTO.News;
using BaoDatShop.DTO.ProductSize;
using BaoDatShop.DTO.ProductType;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IProductSizeService
    {
        public bool Create(CreateProductSize model);
        public bool Update(int id, CreateProductSize model);
        public bool Delete(int id);
        public ProductSize GetById(int id);
        public List<ProductSize> GetAll();
        public List<ProductSize> GetAllProductTypeStatusTrue();
        public List<ProductSize> GetAllProductTypeStatusFalse();
    }
    public class ProductSizeService: IProductSizeService
    {
        private readonly IProductSizeResponsitories IProductSizeResponsitories;

        public ProductSizeService(IProductSizeResponsitories IProductSizeResponsitories)
        {
            this.IProductSizeResponsitories = IProductSizeResponsitories;
          
        }
        public List<ProductSize> GetAllProductTypeStatusFalse()
        {
            return IProductSizeResponsitories.GetAll().Where(a => a.Status == false).ToList();
        }

        public List<ProductSize> GetAllProductTypeStatusTrue()
        {
            return IProductSizeResponsitories.GetAll().Where(a => a.Status == true).ToList();
        }
        public bool Create(CreateProductSize model)
        {
            ProductSize result = new();
            result.Name = model.Name;
            result.ProductId = model.ProductId;
            result.Status = true;
            return IProductSizeResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            ProductSize result = IProductSizeResponsitories.GetById(id);
            result.Status = false;  
            return IProductSizeResponsitories.Update(result);
        }

        public List<ProductSize> GetAll()
        {
            return IProductSizeResponsitories.GetAll();
        }



        public ProductSize GetById(int id)
        {
            var reslut = IProductSizeResponsitories.GetById(id);
     
            return reslut;
        }

        public bool Update(int id, CreateProductSize model)
        {
            ProductSize result = IProductSizeResponsitories.GetById(id);
            result.Name = model.Name;
            result.Status = true;
            result.ProductId = model.ProductId;
            return IProductSizeResponsitories.Update(result);
        }
    }
}
