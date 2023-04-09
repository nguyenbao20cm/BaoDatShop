using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.NewDetail;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Eshop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface INewDetailService
    {
        public bool Create(CreateNewDetail model);
        public bool Update(int id, CreateNewDetail model);
        //public bool Delete(int id);
        public NewDetail GetById(int id);
        public List<NewDetail> GetAll();
    }
    public class NewDetailService : INewDetailService
    {
        private readonly INewDetailResponsitories newDetailResponsitories;
        private readonly IWebHostEnvironment _environment;
        public NewDetailService(IWebHostEnvironment _environment,INewDetailResponsitories newDetailResponsitories)
        {
            this.newDetailResponsitories = newDetailResponsitories;
            this._environment = _environment;
        }

        public bool Create(CreateNewDetail model)
        {
            var fileName = model.Image.FileName;
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "NewDetail");
            var uploadPath = Path.Combine(uploadFolder, fileName);

            using (FileStream fs = System.IO.File.Create(uploadPath))
            {
                model.Image.CopyTo(fs);
                fs.Flush();
            }
            NewDetail result = new();
            result.NewId = model.NewId;
            result.Image = fileName;
            return newDetailResponsitories.Create(result);
        }

        public List<NewDetail> GetAll()
        {
           return newDetailResponsitories.GetAll();
        }

        public NewDetail GetById(int id)
        {
            return newDetailResponsitories.GetById(id);
        }

        public bool Update(int id, CreateNewDetail model)
        {
            var fileName = model.Image.FileName;
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "NewDetail");
            var uploadPath = Path.Combine(uploadFolder, fileName);

            using (FileStream fs = System.IO.File.Create(uploadPath))
            {
                model.Image.CopyTo(fs);
                fs.Flush();
            }
            NewDetail result = newDetailResponsitories.GetById(id);
            result.NewId = model.NewId;
            result.Image = fileName;
            return newDetailResponsitories.Update(result);
        }
    }
}

