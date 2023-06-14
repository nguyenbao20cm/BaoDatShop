using BaoDatShop.DTO.CreateImageProduct;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface IImageProductResponsitories
    {
        public List<ImageProduct> GetAll();
        public ImageProduct GetById(int id);
        public bool Update(ImageProduct model);
        public bool Create(ImageProduct model);
    }
    public class ImageProductResponsitories : IImageProductResponsitories
    {
        private readonly AppDbContext context;
        public ImageProductResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(ImageProduct model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<ImageProduct> GetAll()
        {
            if (context.ImageProduct.ToList() == null) return null;
            return context.ImageProduct.ToList();
        }

        public bool Update(ImageProduct model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public ImageProduct GetById(int id)
        {
            if (context.ImageProduct.Where(a => a.Id == id).FirstOrDefault() == null) return null;
            return context.ImageProduct.Where(a => a.Id == id).FirstOrDefault();
        }

    }
}
