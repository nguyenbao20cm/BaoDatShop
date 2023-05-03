using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.NewDetail;
using BaoDatShop.DTO.News;
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
        public bool Create(CreateNewDetailRequest model);
        public bool Update(int id, CreateNewDetailRequest model);
        public bool Delete(int id);
        public GetAllNewDetailResponse GetById(int id);
        public List<GetAllNewDetailResponse> GetAll();
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

        public bool Create(CreateNewDetailRequest model)
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
            result.Content = model.Content;
            result.Image = fileName;
            return newDetailResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            NewDetail result = newDetailResponsitories.GetById(id);
            result.Status = false;
            return newDetailResponsitories.Update(result);
        }

        public List<GetAllNewDetailResponse> GetAll()
        {
           
            var tamp = newDetailResponsitories.GetAll();
            List<GetAllNewDetailResponse> reslut = new();
            foreach (var item in tamp)
            {
                GetAllNewDetailResponse a = new();
                a.Image = item.Image;
                a.Status = item.Status;
                a.Content = item.Content;
                a.NewDetailId=item.NewDetailId;
                a.NewId = item.NewId;
                reslut.Add(a);
            }
            return reslut;
        }
        public GetAllNewDetailResponse GetById(int id)
        {
            var item = newDetailResponsitories.GetById(id);
            GetAllNewDetailResponse reslut = new();
            reslut.Image = item.Image;
            reslut.Status = item.Status;
            reslut.Content = item.Content;
            reslut.NewDetailId = item.NewDetailId;
            reslut.NewId = item.NewId;
            return reslut;
        }

        public bool Update(int id, CreateNewDetailRequest model)
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
            result.Content = model.Content;
            result.Image = fileName;
            return newDetailResponsitories.Update(result);
        }
    }
}

