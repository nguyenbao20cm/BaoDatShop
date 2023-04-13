using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface INewsResponsitories
    {
        public bool Create(News model);
        public bool Update(News model);
        public News GetById(int id);
        public List<News> GetAll();
    }
    public class NewsResponsitories: INewsResponsitories
    {
        private readonly AppDbContext context;
        public NewsResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(News model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<News> GetAll()
        {
            if (context.News.Where(a=>a.Status==true).ToList() == null) return null;
            return context.News.Where(a => a.Status == true).ToList();
        }

        public News GetById(int id)
        {
            if (context.News.Where(a => a.NewsId == id).Where(a => a.Status == true).FirstOrDefault() == null) return null;
            return context.News.Where(a => a.NewsId == id).FirstOrDefault();
        }

        public bool Update(News model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
