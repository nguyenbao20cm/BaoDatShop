using BaoDatShop.DTO.News;
using BaoDatShop.DTO.Product;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Eshop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface INewsService
    {
        public bool Create(CreateNewsRequest model);
        public bool Update(int id, CreateNewsRequest model);
        public bool Delete(int id);
        public GetAllNewResponse GetById(int id);
        public List<GetAllNewResponse> GetAll();
    }
    public class NewsService: INewsService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly INewsResponsitories INewsResponsitories;
        public NewsService(INewsResponsitories INewsResponsitories,IWebHostEnvironment _environment)
        {
            this._environment = _environment;
            this.INewsResponsitories = INewsResponsitories;
        }

        public bool Create(CreateNewsRequest model)
        {
            var fileName = model.Image.FileName;
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "New");
            var uploadPath = Path.Combine(uploadFolder, fileName);

            using (FileStream fs = System.IO.File.Create(uploadPath))
            {
                model.Image.CopyTo(fs);
                fs.Flush();
            }
            News result = new();
            result.Image = fileName;
            result.NewsName = model.NewsName;
            result.DateTime = DateTime.Now;
            result.Content = model.Content;
            result.Status = true;
            return INewsResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            News result = INewsResponsitories.GetById(id);
            result.Status = false;
            return INewsResponsitories.Update(result);
        }

        public List<GetAllNewResponse> GetAll()
        {
            var tamp = INewsResponsitories.GetAll();
            List<GetAllNewResponse> reslut = new();
            foreach (var item in tamp)
            {
                GetAllNewResponse a = new();
                a.Image = item.Image;
                a.Status = item.Status;
                a.Content = item.Content;
                a.DateTime = item.DateTime;
                a.NewsId = item.NewsId;
                a.NewsName = item.NewsName;
                reslut.Add(a);
            }
            return reslut;
        
        }

        public GetAllNewResponse GetById(int id)
        {
            var tamp = INewsResponsitories.GetById(id);
            GetAllNewResponse result = new();
            result.Status = tamp.Status;
            result.Content = tamp.Content;
            result.DateTime = tamp.DateTime;
            result.NewsId = tamp.NewsId;
            result.NewsName = tamp.NewsName;
            return result;
        }

        public bool Update(int id, CreateNewsRequest model)
        {
            var fileName = model.Image.FileName;
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "New");
            var uploadPath = Path.Combine(uploadFolder, fileName);

            using (FileStream fs = System.IO.File.Create(uploadPath))
            {
                model.Image.CopyTo(fs);
                fs.Flush();
            }
            News result = INewsResponsitories.GetById(id);
            result.Image = fileName;
            result.NewsName = model.NewsName;
            result.DateTime = DateTime.Now;
            result.Content = model.Content;
            return INewsResponsitories.Update(result);
        }
    }
}
