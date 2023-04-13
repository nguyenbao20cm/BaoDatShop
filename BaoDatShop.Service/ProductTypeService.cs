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
        public bool Create(CreateProductType model);
        public bool Update(int id,CreateProductType model);
        public bool Delete(int id);
        public GetAllProductType GetById(int id);
        public List<GetAllProductType> GetAll();
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

        public List<GetAllProductType> GetAll()
        {
         
            var tamp = productTypeResponsitories.GetAll();
            List<GetAllProductType> reslut = new();
            foreach (var item in tamp)
            {
                GetAllProductType a = new();
                a.Name = item.Name;
                a.Id=item.Id;
                a.Status = item.Status;
                reslut.Add(a);
            }
            return reslut;
        }

        public GetAllProductType GetById(int id)
        {
            var tamp = productTypeResponsitories.GetById(id);
            GetAllProductType reslut = new();
            reslut.Name = tamp.Name;
            reslut.Id = tamp.Id;
            reslut.Status = tamp.Status;
            return reslut;
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
