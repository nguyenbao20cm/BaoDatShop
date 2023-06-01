using BaoDatShop.DTO.Product;
using BaoDatShop.Responsitories;
using Eshop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace BaoDatShop.Service
{
    public interface IProductService
    {
        
        public bool CreateImageProduct(IFormFile model);
        public bool Create(CreateProductRequest model);
        public bool Update(int id, CreateProductRequest model);
        public bool Delete(int id);
        public GetAllProductResponse GetById(int id);
        public List<Product> GetAll();
        public Product GetByName(string name);
        public List<Product> GetAllProductStatusFalse();
        public List<Product> GetAllProductStatusTrue();
        
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

        public List<Product> GetAllProductStatusFalse()
        {
            return productResponsitories.GetAll().Where(a => a.Status == false).ToList();
        }

        public List<Product> GetAllProductStatusTrue()
        {
            return productResponsitories.GetAll().Where(a => a.Status == true).ToList();
        }
        public bool Create(CreateProductRequest model)
        {
            //var fileName = model.Image.FileName;
            ////var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Product");

            //var uploadFolder = Path.Combine("C:\\Users\\ADMIN\\OneDrive\\Desktop\\AdminBaoDatShop\\my-app\\src\\Assets\\images");
            //var uploadPath = Path.Combine(uploadFolder, fileName);

            //using (FileStream fs = System.IO.File.Create(uploadPath))
            //{
            //    model.Image.CopyTo(fs);
            //    fs.Flush();
            //}
            Product result = new();
      
            result.Name = model.Name;
            result.Description = model.Description;
            result.Price = model.Price;
            result.Stock = model.Stock;
            result.ProductTypeId = model.ProductTypeId;
            result.Image = model.Image;
            result.Status = true;
            return productResponsitories.Create(result);
        }

        public bool CreateImageProduct(IFormFile Image)
        {
            if (Image != null)
            {
                var fileName = Image.FileName;
                //var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Product");
           
                      var uploadFolder = Path.Combine("C:\\Users\\ADMIN\\OneDrive\\Desktop\\admin\\src\\assets\\images\\products");
                var uploadPath = Path.Combine(uploadFolder, fileName);

                using (FileStream fs = System.IO.File.Create(uploadPath))
                {
                    Image.CopyTo(fs);
                    fs.Flush();
                }
                return true;
            }
            return false;
        
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

        public List<GetAllProductResponse> GetAllProductInProductType(int id)
        {
            var tamp = productResponsitories.GetAll().Where(a=>a.ProductTypeId==id);
            List<GetAllProductResponse> reslut = new();
            foreach (var item in tamp)
            {
                GetAllProductResponse a = new();
                a.Id = item.Id;
             
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
            ////var fileName = model.Image.FileName;
            ////var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Product");
            ////var uploadPath = Path.Combine(uploadFolder, fileName);

            ////using (FileStream fs = System.IO.File.Create(uploadPath))
            ////{
            ////    model.Image.CopyTo(fs);
            ////    fs.Flush();
            ////}

            //var fileName = model.Image.FileName;
            ////var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Product");

            //var uploadFolder = Path.Combine("C:\\Users\\ADMIN\\OneDrive\\Desktop\\AdminBaoDatShop\\my-app\\src\\Assets\\images");
            //var uploadPath = Path.Combine(uploadFolder, fileName);

            //using (FileStream fs = System.IO.File.Create(uploadPath))
            //{
            //    model.Image.CopyTo(fs);
            //    fs.Flush();
            //}
            Product result = productResponsitories.GetById(id);
        
            result.Name = model.Name;
            result.Description = model.Description;
            result.Price = model.Price;
            result.Stock = model.Stock;
            result.ProductTypeId = model.ProductTypeId;
            result.Image = model.Image;
            return productResponsitories.Update(result);
        }

        public Product GetByName(string name)
        {
            return productResponsitories.GetAll().Where(a => a.Name == name).FirstOrDefault();
        }
    }
}
