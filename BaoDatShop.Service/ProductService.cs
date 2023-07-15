using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Product;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Eshop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using static System.Net.Mime.MediaTypeNames;

namespace BaoDatShop.Service
{
    public interface IProductService
    {
        
        public bool CreateImageProduct(IFormFile model);
        public bool Create(string id,CreateProductRequest model);
        public bool Update(int id,string ida, CreateProductRequest model);
        public bool Delete(int id);
        public Product GetById(int id);
        public List<Product> GetAll();
        public Product GetByName(string name);
        public List<Product> GetAllProductStatusFalse();
        public List<Product> GetAllProductStatusTrue();
   
        public List<Product> GetAllProductInProductType(int id);
    }
    public class ProductService : IProductService
    {
        private readonly IProductResponsitories productResponsitories;
        private readonly IWebHostEnvironment _environment;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        public ProductService(IProductResponsitories productResponsitories, IWebHostEnvironment _environment,
            IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.productResponsitories = productResponsitories;
            this._environment = _environment;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
        }

        public List<Product> GetAllProductStatusFalse()
        {
            return productResponsitories.GetAll().Where(a => a.Status == false).ToList();
        }

        public List<Product> GetAllProductStatusTrue()
        {
            return productResponsitories.GetAll().Where(a => a.BrandProduct.Status == true).Where(a=>a.ProductType.Status==true).Where(a => a.Status == true).ToList();
        }
        public bool Create(string id,CreateProductRequest model)
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
            result.BrandProductId = model.BrandProductId;
            result.Name = model.Name;
            result.PriceSales = model.PriceSales;
            result.SKU = model.SKU;
            result.Description = model.Description;
            result.Price = model.Price;
            result.CountSell = 0;
            result.ProductTypeId = model.ProductTypeId;
            result.Image = model.Image;
            result.Status = model.Status;
            bool ab = productResponsitories.Create(result);
            if (ab == true)
            {
                //var tam = productResponsitories.GetById(result.Id);
                //HistoryAccount a = new();
                //a.AccountID = id; a.Datetime = DateTime.Now;
                //a.Content = "Đã thêm sản phẩm " + model.Name +" thuộc loại sản phẩm "+tam.ProductType.Name;
                //IHistoryAccountResponsitories.Create(a);
            }
            return ab;
        }

        public bool CreateImageProduct(IFormFile Image)
        {
            if (Image != null)
            {
                var fileName = Image.FileName;
                //var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Product");
                var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Product");
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

        public List<Product> GetAllProductInProductType(int id)
        {
            var tamp = productResponsitories.GetAll().Where(a=>a.ProductTypeId==id).ToList();
          
            return tamp;
        }



        public Product GetById(int id)
        {
            var item = productResponsitories.GetById(id);
           
            return item;
        }

        public bool Update(int id,string ida, CreateProductRequest model)
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
            result.PriceSales = model.PriceSales;
            result.SKU = model.SKU;
            result.Description = model.Description;
            result.Price = model.Price;
            result.BrandProductId = model.BrandProductId;
            result.ProductTypeId = model.ProductTypeId;
            result.Image = model.Image;
            result.Status = model.Status;
            bool ab = productResponsitories.Update(result);
            if (ab == true)
            {
                //var tam = productResponsitories.GetById(result.Id);
                //HistoryAccount a = new();
                //a.AccountID = ida; a.Datetime = DateTime.Now;
                //a.Content = "Đã chỉnh sửa sản phẩm " + model.Name + " thuộc loại sản phẩm " + tam.ProductType.Name;
                //IHistoryAccountResponsitories.Create(a);
            }
            return ab;
        }

        public Product GetByName(string name)
        {
            return productResponsitories.GetAll().Where(a => a.Name == name).FirstOrDefault();
        }

     
    }
}
