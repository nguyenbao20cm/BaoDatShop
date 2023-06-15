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
    public interface IDisscountRespositories
    {
        public bool Create(Disscount model);
        public bool Update(Disscount model);
        public Disscount GetById(int id);
        public List<Disscount> GetAll();
    }
    public class DisscountRespositories : IDisscountRespositories
    {
        private readonly AppDbContext context;
        public DisscountRespositories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(Disscount model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<Disscount> GetAll()
        {
            if (context.Disscount.ToList() == null) return null;
            return context.Disscount.Include(a=>a.Product).ToList();
        }



        public Disscount GetById(int id)
        {
            if (context.Disscount.Where(a => a.Id == id).FirstOrDefault() == null) return null;
            return context.Disscount.Include(a => a.Product).Where(a => a.Id == id).FirstOrDefault();
        }

        public bool Update(Disscount model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
