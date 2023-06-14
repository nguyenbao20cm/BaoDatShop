using BaoDatShop.DTO.CreateImageProduct;
using BaoDatShop.DTO.Disscount;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BaoDatShop.Service
{
    public interface IImageProductService
    {
        public bool Create(CreateImageProduct model);
        public bool Delete(int id);
        public bool Update(int id, CreateImageProduct model);
        public List<ImageProduct> GetAll();
        public List<ImageProduct> GetAllImageProductStatusTrue();
        public List<ImageProduct> GetAllImageProductStatusFalse();
        public bool CreateImage(IFormFile model);
        public ImageProduct GetById(int id);
        
    }
    public class ImageProductService : IImageProductService
    {
        private readonly IImageProductResponsitories IImageProductResponsitories;
        public ImageProductService(IImageProductResponsitories IImageProductResponsitories)
        {
            this.IImageProductResponsitories = IImageProductResponsitories;
        }
        public bool Create(CreateImageProduct model)
        {
            ImageProduct result = new();
            result.ProductId = model.ProductId;
            result.Status = model.Status;
            result.Image = model.Image;
            return IImageProductResponsitories.Create(result);
        }

        public bool CreateImage(IFormFile model)
        {
            if (model == null) return false;
            var fileName = model.FileName;
            var uploadFolder = "C:\\Users\\ADMIN\\OneDrive\\Desktop\\admin\\src\\assets\\images\\ImgaeProduct";
            var uploadPath = Path.Combine(uploadFolder, fileName);

            using (FileStream fs = System.IO.File.Create(uploadPath))
            {
                model.CopyTo(fs);
                fs.Flush();
            }
            return true;
        }

        public bool Delete(int id)
        {
            ImageProduct result = IImageProductResponsitories.GetById(id);
            result.Status = false;
            return IImageProductResponsitories.Update(result);
        }

        public List<ImageProduct> GetAll()
        {
            return IImageProductResponsitories.GetAll().ToList();
        }

        public List<ImageProduct> GetAllImageProductStatusFalse()
        {
            return IImageProductResponsitories.GetAll().Where(a=>a.Status==false).ToList();
        }

        public List<ImageProduct> GetAllImageProductStatusTrue()
        {
            return IImageProductResponsitories.GetAll().Where(a => a.Status == true).ToList();
        }

        public ImageProduct GetById(int id)
        {
            return IImageProductResponsitories.GetById(id);
        }

        public bool Update(int id, CreateImageProduct model)
        {
            ImageProduct result = IImageProductResponsitories.GetById(id);
            result.ProductId = model.ProductId;
            result.Status = model.Status;
            result.Image = model.Image;
            return IImageProductResponsitories.Update(result);
        }
    }
}
