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
    public interface IKhoHangResposirity
    {
        public List<KHoHang> GetAll();
        public KHoHang GetById(int id);
    }
    public class KhoHangResposirity : IKhoHangResposirity
    {
        private readonly AppDbContext context;
        public KhoHangResposirity(AppDbContext context)
        {
            this.context = context;
        }

        public List<KHoHang> GetAll()
        {
            return context.KHoHang.Include(a=>a.ProductSize.Product).Include(a => a.ProductSize).ToList();
        }

        public KHoHang GetById(int id)
        {
            return context.KHoHang.Include(a => a.ProductSize.Product).Include(a => a.ProductSize).Where(a=>a.Id==id).FirstOrDefault();
        }
    }
}
