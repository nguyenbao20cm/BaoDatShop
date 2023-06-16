using BaoDatShop.Model.Context;
using Eshop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface IInvoiceDetailResponsitories
    {
        public bool Create(InvoiceDetail model);
        public bool Update(InvoiceDetail model);
        public InvoiceDetail GetById(int id);
        public List<InvoiceDetail> GetAll(int id);
    }

    public class InvoiceDetailResponsitories : IInvoiceDetailResponsitories
    {
        private readonly AppDbContext context;
        public InvoiceDetailResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(InvoiceDetail model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<InvoiceDetail> GetAll(int id)
        {
            if (context.InvoiceDetail.Where(a=>a.InvoiceId==id).ToList() == null) return null;
            return context.InvoiceDetail.Include(a=>a.Product).Where(a => a.InvoiceId == id).ToList();
        }

      

        public InvoiceDetail GetById(int id)
        {
            if (context.InvoiceDetail.Where(a => a.Id == id).FirstOrDefault() == null) return null;
            return context.InvoiceDetail.Where(a => a.Id == id).FirstOrDefault();
        }

        public bool Update(InvoiceDetail model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
