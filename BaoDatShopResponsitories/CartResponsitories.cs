using BaoDatShop.Model.Context;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{   
    public interface ICartResponsitories
    {
        public bool DeleteAll(int id);
        public bool Delete(int id);
        public bool Create(Cart model);
        public bool Update(Cart model);
        public Cart GetById(int id);
        public List<Cart> GetAll();
    }
    public class CartResponsitories : ICartResponsitories
    {
        private readonly AppDbContext context;
        public CartResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(Cart model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public bool Delete(int id)
        {
            var reslut = context.Cart.Where(a => a.Id == id).FirstOrDefault();
            if (reslut == null) return false;
            context.Remove(reslut);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
        public bool DeleteAll(int id)
        {
          var a=  context.Cart.ToList();
            foreach(var item in a)
            {
                if (item.AccountId == id)
                context.Remove(item);context.SaveChanges(); 
            }  
           return true;
        }

        public List<Cart> GetAll()
        {
            if (context.Cart.ToList() == null) return null;
            return context.Cart.ToList();
        }

        public Cart GetById(int id)
        {
            if (context.Cart.Where(a => a.Id == id).FirstOrDefault() == null) return null;
            return context.Cart.Where(a => a.Id == id).FirstOrDefault();
        }

        public bool Update(Cart model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
