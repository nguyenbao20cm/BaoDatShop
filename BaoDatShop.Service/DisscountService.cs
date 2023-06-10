using BaoDatShop.DTO.AdvertisingPanel;
using BaoDatShop.DTO.Disscount;
using BaoDatShop.DTO.InvoiceDetail;
using BaoDatShop.DTO.ProductType;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IDisscountService
    {
        public bool Create(CreateDisscount model);
        public bool Delete(int id);
        public bool Update(int id, CreateDisscount model);
        public List<Disscount> GetAll();
        public List<Disscount> GetAllDisscountPanel();
        public List<Disscount> GetAllDisscountStatusFalse();
    }
    public class DisscountService : IDisscountService
    {
        private readonly IDisscountRespositories IDisscountRespositories;
    
        public DisscountService(IDisscountRespositories IDisscountRespositories)
        {
            this.IDisscountRespositories = IDisscountRespositories;
        }
        public List<Disscount> GetAllDisscountStatusFalse()
        {
            return IDisscountRespositories.GetAll().Where(a => a.Status == false).ToList();
        }

        public List<Disscount> GetAllDisscountPanel()
        {
            return IDisscountRespositories.GetAll().Where(a => a.Status == true).ToList();
        }
        public bool Create(CreateDisscount model)
        {
            Disscount result = new();
            result.NameDisscount = model.NameDisscount;
            result.Status = model.Status;
            result.ProductId = model.ProductId;
            return IDisscountRespositories.Create(result);
        }

        public bool Delete(int id)
        {
            
                Disscount result = IDisscountRespositories.GetById(id);
                result.Status = false;
  
                return IDisscountRespositories.Update(result);
        }

        public List<Disscount> GetAll()
        {
            return IDisscountRespositories.GetAll();
        }



        public Disscount GetById(int id)
        {
            var reslut = IDisscountRespositories.GetById(id);
            return reslut;
        }

        public bool Update(int id, CreateDisscount model)
        {
            Disscount result = IDisscountRespositories.GetById(id);
            result.NameDisscount = model.NameDisscount;
            result.Status = model.Status;
            result.ProductId = model.ProductId;
            return IDisscountRespositories.Update(result);
        }
    }
}
