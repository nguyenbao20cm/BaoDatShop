using BaoDatShop.DTO.AdvertisingPanel;
using BaoDatShop.DTO.Cart;
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
    public interface IAdvertisingPanelService
    {
        public bool Create(CreateAdvertisingPanelRequest model);
        public bool Delete(int id);
        public bool Update(int id, CreateAdvertisingPanelRequest model);
        public List<AdvertisingPanel> GetAll();
        public List<AdvertisingPanel> GetAllAdvertisingPanel();
        public List<AdvertisingPanel> GetAllAdvertisingPanelStatusFalse();
        
    }
    public class AdvertisingPanelService : IAdvertisingPanelService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IAdvertisingPanelResponsitories advertisingPanelResponsitories;
        public AdvertisingPanelService(IWebHostEnvironment _environment, IAdvertisingPanelResponsitories advertisingPanelResponsitories)
        {
            this._environment = _environment;
            this.advertisingPanelResponsitories = advertisingPanelResponsitories;
        }

        public bool Create(CreateAdvertisingPanelRequest model)
        {
            var fileName = model.Image.FileName;
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "AdvertisingPanel");
            var uploadPath = Path.Combine(uploadFolder, fileName);

            using (FileStream fs = System.IO.File.Create(uploadPath))
            {
                model.Image.CopyTo(fs);
                fs.Flush();
            }
            AdvertisingPanel result = new();
            result.Image = fileName;
            result.Status = true;
           return advertisingPanelResponsitories.Create(result);
        }

        public bool Delete(int id)
        {
            AdvertisingPanel result = advertisingPanelResponsitories.GetById(id);
            result.Status = false;
            return advertisingPanelResponsitories.Update(result);
        }

        public List<AdvertisingPanel> GetAll()
        {
            return advertisingPanelResponsitories.GetAll().Where(a=>a.Status==true).ToList();
        }

        public List<AdvertisingPanel> GetAllAdvertisingPanelStatusFalse()
        {
            return advertisingPanelResponsitories.GetAll().Where(a => a.Status == false).ToList();
        }

        public List<AdvertisingPanel> GetAllAdvertisingPanel()
        {
            return advertisingPanelResponsitories.GetAll().ToList();
        }

        public bool Update(int id, CreateAdvertisingPanelRequest model)
        {
            var fileName = model.Image.FileName;
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "AdvertisingPanel");
            var uploadPath = Path.Combine(uploadFolder, fileName);

            using (FileStream fs = System.IO.File.Create(uploadPath))
            {
                model.Image.CopyTo(fs);
                fs.Flush();
            }
            AdvertisingPanel result = advertisingPanelResponsitories.GetById(id);
            result.Image = fileName;
            return advertisingPanelResponsitories.Update(result);
        }
    }
}
