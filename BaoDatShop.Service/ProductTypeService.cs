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
        public bool Create(CreateProductType model);
        public bool Update(int id,CreateProductType model);
        public bool Delete(int id);
        public ProductTypes GetById(int id);
        public List<ProductTypes> GetAll();
    }
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeResponsitories productTypeResponsitories;
        public ProductTypeService(IProductTypeResponsitories productTypeResponsitories)
        {
            this.productTypeResponsitories=productTypeResponsitories;
        }
        public bool Create(CreateProductType model)
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
            return productTypeResponsitories.Update(result);
        }

        public List<ProductTypes> GetAll()
        {
            return productTypeResponsitories.GetAll();
        }

        public ProductTypes GetById(int id)
        {
            return productTypeResponsitories.GetById(id);
        }

        public bool Update(int id,CreateProductType model)
        {
            ProductTypes result = productTypeResponsitories.GetById(id);
            result.Name = model.Name;
            result.Status = true;
            return productTypeResponsitories.Update(result);
        }
    }
}
