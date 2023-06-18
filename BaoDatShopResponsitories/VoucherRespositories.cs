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
        public Voucher GetById(int id);
        public Voucher GetByName(string Name);
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

        public Voucher GetByName(string Name)
        {
            if (context.Voucher.Where(a => a.Name == Name).ToList() == null) return null;
            return context.Voucher.Where(a => a.Name == Name).FirstOrDefault();
        }

        public bool Update(Voucher model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

    }
}
