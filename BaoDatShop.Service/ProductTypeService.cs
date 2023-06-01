using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.ProductType;
using BaoDatShop.Responsitories;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IProductTypeService
    {
        public bool Create(CreateProductTypeRequest model);
        public bool Update(int id, CreateProductTypeRequest model);
        public bool Delete(int id);
        public GetAllProductTypeResponse GetById(int id);
        public List<ProductTypes> GetAll();
        public List<ProductTypes> GetAllProductTypeStatusTrue();
        public List<ProductTypes> GetAllProductTypeStatusFalse();
    }
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeResponsitories productTypeResponsitories;
        private readonly IProductResponsitories productResponsitories;
        public ProductTypeService(IProductTypeResponsitories productTypeResponsitories, IProductResponsitories productResponsitories)
        {
            this.productTypeResponsitories=productTypeResponsitories;
            this.productResponsitories= productResponsitories;
        }
        public List<ProductTypes> GetAllProductTypeStatusFalse()
        {
            return productTypeResponsitories.GetAll().Where(a => a.Status == false).ToList();
        }

        public List<ProductTypes> GetAllProductTypeStatusTrue()
        {
            return productTypeResponsitories.GetAll().Where(a=>a.Status==true).ToList();
        }
        public bool Create(CreateProductTypeRequest model)
        {
            ProductTypes result = new();
            result.Name = model.Name;
            result.Status = true;
            return productTypeResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            ProductTypes result = productTypeResponsitories.GetById(id);
            result.Status = false;
            var Pro=productResponsitories.GetAll(); 
            foreach(var a in Pro)
            {
                if (a.ProductTypeId == id)
                    a.Status = false;
                productResponsitories.Update(a);
            }
            return productTypeResponsitories.Update(result);
        }

        public List<ProductTypes> GetAll()
        {
            return productTypeResponsitories.GetAll();
        }

     

        public GetAllProductTypeResponse GetById(int id)
        {
            var tamp = productTypeResponsitories.GetById(id);
            GetAllProductTypeResponse reslut = new();
            reslut.Name = tamp.Name;
            reslut.Id = tamp.Id;
            reslut.Status = tamp.Status;
            return reslut;
        }

        public bool Update(int id,CreateProductTypeRequest model)
        {
            ProductTypes result = productTypeResponsitories.GetById(id);
            result.Name = model.Name;
            result.Status = true;
            return productTypeResponsitories.Update(result);
        }
    }
}
