using BaoDatShop.DTO.CreateFavoriteProduct;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface IFavoriteProductRespositories
    {
        public bool Create(FavoriteProduct model);
        public bool Update(FavoriteProduct model);
        public bool Delete(string idacc, int id );
        public List<FavoriteProduct> GetAll();
        public FavoriteProduct GetbyId(int id);

    }
    public class FavoriteProductResponsitories : IFavoriteProductRespositories
    {
        private readonly AppDbContext context;
        public FavoriteProductResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(FavoriteProduct model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public bool Delete(string idacc, int id)
        {
            var a = context.FavoriteProduct.Where(a=>a.AccountId==idacc).Where(a => a.ProductId == id).FirstOrDefault();
            if (a == null) return false;
            context.Remove(a);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<FavoriteProduct> GetAll()
        {
            if (context.FavoriteProduct.ToList() == null) return null;
            return context.FavoriteProduct.Include(a => a.Account).Include(a => a.Product).ToList();
        }

        public FavoriteProduct GetbyId(int id)
        {
            if (context.FavoriteProduct.ToList() == null) return null;
            return context.FavoriteProduct.Include(a => a.Account).Include(a => a.Product).Where(a=>a.Id==id).FirstOrDefault();
        }

        public bool Update(FavoriteProduct model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

       
    }
}
