using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface INewDetailResponsitories
    {
        public bool Create(NewDetail model);
        public bool Update(NewDetail model);
        public NewDetail GetById(int id);
        public List<NewDetail> GetAll();
    }
    public class NewDetailResponsitories: INewDetailResponsitories
    {
        private readonly AppDbContext context;
        public NewDetailResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(NewDetail model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<NewDetail> GetAll()
        {
            if (context.NewDetail.ToList() == null) return null;
            return context.NewDetail.ToList();
        }

        public NewDetail GetById(int id)
        {
            if (context.NewDetail.Where(a => a.NewDetailId == id).FirstOrDefault() == null) return null;
            return context.NewDetail.Where(a => a.NewDetailId == id).FirstOrDefault();
        }

        public bool Update(NewDetail model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
