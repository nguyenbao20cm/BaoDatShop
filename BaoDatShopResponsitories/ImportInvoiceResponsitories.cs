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
    public interface IImportInvoiceResponsitories
    {
        public bool Create(ImportInvoice model);
        public bool Update(ImportInvoice model);
        public bool Delete(int id );
        public ImportInvoice GetById(int id);
        public List<ImportInvoice> GetAll();
    }
    public class ImportInvoiceResponsitories: IImportInvoiceResponsitories
    {
        private readonly AppDbContext context;
        public ImportInvoiceResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(ImportInvoice model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public bool Delete(int id)
        {
            var a = GetById(id);
            context.Remove(a);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<ImportInvoice> GetAll()
        {
            if (context.ImportInvoice.ToList() == null) return null;
            return context.ImportInvoice.Include(a => a.ProductSize.Product).Include(a => a.ProductSize).Include(a => a.Supplier).ToList();
        }

        public ImportInvoice GetById(int id)
        {
            if (context.ImportInvoice.Where(a => a.Id == id).FirstOrDefault() == null) return null;
            return context.ImportInvoice.Include(a => a.ProductSize.Product).Include(a => a.ProductSize).Include(a => a.Supplier).Where(a => a.Id == id).FirstOrDefault();
        }

        public bool Update(ImportInvoice model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
