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
    public interface IReviewResponsitories
    {
        public bool Create(Review model);
        public bool Update(Review model);
        public bool Delete(int id );
        public Review GetById(int id);
        public List<Review> GetAll();
    }
    public class ReviewResponsitories: IReviewResponsitories
    {
        private readonly AppDbContext context;
        public ReviewResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(Review model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public bool Delete(int id)
        {
            context.Remove(context.Review.Where(a => a.ReviewId == id).FirstOrDefault());
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<Review> GetAll()
        {
            if (context.Review.ToList() == null) return null;
            return context.Review.Include(a=>a.Account).Include(a=>a.Product).ToList();
        }

        public Review GetById(int id)
        {
            if (context.Review.Where(a => a.ReviewId == id).FirstOrDefault() == null) return null;
            return context.Review.Where(a => a.ReviewId == id).FirstOrDefault();
        }

        public bool Update(Review model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
