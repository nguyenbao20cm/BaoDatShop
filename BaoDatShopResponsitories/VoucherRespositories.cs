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
    public interface IVoucherRespositories
    {
        public bool Create(Voucher model);
        public bool Update(Voucher model);
        public bool Delete(int id );
        public Voucher GetById(int id);
        public Voucher GetByName(int total,string Name);
        public List<Voucher> GetAll();
    }
    public class VoucherRespositories : IVoucherRespositories
    {
        private readonly AppDbContext context;
        public VoucherRespositories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(Voucher model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public bool Delete(int id)
        {
            var a = context.Voucher.Where(a => a.Id == id).FirstOrDefault();
            a.Status = false;
            context.Update(a);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<Voucher> GetAll()
        {
            if (context.Voucher.ToList() == null) return null;
            return context.Voucher.ToList();
        }

        public Voucher GetById(int id)
        {
            if (context.Voucher.ToList() == null) return null;
            return context.Voucher.Where(a => a.Id == id).FirstOrDefault();
        }

        public Voucher GetByName(int total,string Name)
        {
            if (context.Voucher.Where(a => a.Name == Name).ToList() == null) return null;
            return context.Voucher.Where(a => a.Name == Name).Where(a=>a.Status==true).Where(a=>a.EndDay>DateTime.Now).Where(a=>a.MinMoney<total).FirstOrDefault();
        }

        public bool Update(Voucher model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

    }
}
