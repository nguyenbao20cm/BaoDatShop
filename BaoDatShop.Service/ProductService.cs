using BaoDatShop.DTO.Product;
using BaoDatShop.Responsitories;
using Eshop.Models;
using Microsoft.AspNetCore.Hosting;

namespace BaoDatShop.Service
{
    public interface IProductService
    {
        public bool Create(CreateProductRequest model);
        public bool Update(int id, CreateProductRequest model);
        public bool Delete(int id);
        public GetAllProductResponse GetById(int id);
        public List<GetAllProductResponse> GetAll();
        
        public List<GetAllProductResponse> GetAllProductInProductType(int id);
    }
    public class ProductService : IProductService
    {
        private readonly IProductResponsitories productResponsitories;
        private readonly IWebHostEnvironment _environment;
        public ProductService(IProductResponsitories productResponsitories, IWebHostEnvironment _environment)
        {
            this.productResponsitories = productResponsitories;
            this._environment = _environment;
        }

        public bool Create(CreateProductRequest model)
        {
            var fileName = model.Image.FileName;
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "NewDetail");
            var uploadPath = Path.Combine(uploadFolder, fileName);

            using (FileStream fs = System.IO.File.Create(uploadPath))
            {
                model.Image.CopyTo(fs);
                fs.Flush();
            }
            Product result = new();
            result.SKU = model.SKU;
            result.Name = model.Name;
            result.Description = model.Description;
            result.Price = model.Price;
            result.Stock = model.Stock;
            result.ProductTypeId = model.ProductTypeId;
            result.Image = fileName;
            result.Status = true;
            return productResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            Product result = productResponsitories.GetById(id);
            result.Status = false;
            return productResponsitories.Update(result);
        }

        public List<GetAllProductResponse> GetAll()
        {
            var tamp = productResponsitories.GetAll();
            List<GetAllProductResponse> reslut = new();
            foreach (var item in tamp)
            {
                GetAllProductResponse a = new();
                a.Id = item.Id;
                a.SKU = item.SKU;
                a.Name = item.Name;
                a.Description = item.Description;
                a.Price = item.Price;
                a.Stock = item.Stock;
                a.ProductTypeId = item.ProductTypeId;
                a.Image = item.Image;
                reslut.Add(a);
            }
            return reslut;
        }

        public List<GetAllProductResponse> GetAllProductInProductType(int id)
        {
            var tamp = productResponsitories.GetAll().Where(a=>a.ProductTypeId==id);
            List<GetAllProductResponse> reslut = new();
            foreach (var item in tamp)
            {
                GetAllProductResponse a = new();
                a.Id = item.Id;
                a.SKU = item.SKU;
                a.Name = item.Name;
                a.Description = item.Description;
                a.Price = item.Price;
                a.Stock = item.Stock;
                a.ProductTypeId = item.ProductTypeId;
                a.Image = item.Image;
                reslut.Add(a);
            }
            return reslut;
        }

        public GetAllProductResponse GetById(int id)
        {
            var item = productResponsitories.GetById(id);
            GetAllProductResponse a = new();
            a.Id = item.Id;
            a.SKU = item.SKU;
            a.Name = item.Name;
            a.Description = item.Description;
            a.Price = item.Price;
            a.Stock = item.Stock;
            a.ProductTypeId = item.ProductTypeId;
            a.Image = item.Image;
            return a;
        }

        public bool Update(int id, CreateProductRequest model)
        {
            var fileName = model.Image.FileName;
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "NewDetail");
            var uploadPath = Path.Combine(uploadFolder, fileName);

            using (FileStream fs = System.IO.File.Create(uploadPath))
            {
                model.Image.CopyTo(fs);
                fs.Flush();
            }
            Product result = productResponsitories.GetById(id);
            result.SKU = model.SKU;
            result.Name = model.Name;
            result.Description = model.Description;
            result.Price = model.Price;
            result.Stock = model.Stock;
            result.ProductTypeId = model.ProductTypeId;
            result.Image = fileName;
            return productResponsitories.Update(result);
        }
    }
}
