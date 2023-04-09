using BaoDatShop.Model.Context;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface IInvoiceResponsitories
    {
        public bool Create(Invoice model);
        public bool Update(Invoice model);
        public Invoice GetById(int id);
        public List<Invoice> GetAll();
    }
    public class InvoiceResponsitories: IInvoiceResponsitories
    {
        private readonly AppDbContext context;
        public InvoiceResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(Invoice model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<Invoice> GetAll()
        {
            if (context.Invoice.ToList() == null) return null;
            return context.Invoice.ToList();
        }

        public Invoice GetById(int id)
        {
            if (context.Invoice.Where(a => a.Id == id).FirstOrDefault() == null) return null;
            return context.Invoice.Where(a => a.Id == id).FirstOrDefault();
        }

        public bool Update(Invoice model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
