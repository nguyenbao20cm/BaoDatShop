using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Eshop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface IAdvertisingPanelResponsitories
    {
        public List<AdvertisingPanel> GetAll();
        public AdvertisingPanel GetById(int id);
        public bool Update( AdvertisingPanel model);
        public bool Create(AdvertisingPanel model);
    }
    public class AdvertisingPanelResponsitories : IAdvertisingPanelResponsitories
    {
        private readonly AppDbContext context;
        public AdvertisingPanelResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(AdvertisingPanel model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<AdvertisingPanel> GetAll()
        {
            if (context.AdvertisingPanel.Where(a => a.Status == true).ToList() == null) return null;
            return context.AdvertisingPanel.Where(a => a.Status == true).ToList();
        }

        public bool Update(AdvertisingPanel model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public AdvertisingPanel GetById( int id )
        {
            if (context.AdvertisingPanel.Where(a => a.AdvertisingPanelID == id).Where(a => a.Status == true).FirstOrDefault() == null) return null;
            return context.AdvertisingPanel.Where(a => a.AdvertisingPanelID == id).FirstOrDefault();
        }

        
    }
}
