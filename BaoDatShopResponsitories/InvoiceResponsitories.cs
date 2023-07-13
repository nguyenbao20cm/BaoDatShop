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
            if (context.Invoice.Where(a => a.Status == true).ToList() == null) return null;
            return context.Invoice.Include(a=>a.Account).Where(a => a.Status == true).ToList();


            var query = context.Invoice
               .GroupBy(hoadon => hoadon.IssuedDate.Date)
               .Select(g => new
               {
                   NgayLap = g.Key,
                   TongTien = g.Sum(hd => hd.Total)
               });

            foreach (var result in query)
            {
                Console.WriteLine($"Ngày: {result.NgayLap.ToShortDateString()}, Tổng tiền: {result.TongTien}");
            }
        }

        public Invoice GetById(int id)
        {
            if (context.Invoice.Where(a => a.Id == id).Where(a=>a.Status==true).FirstOrDefault() == null) return null;
            return context.Invoice.Where(a => a.Id == id).Where(a => a.Status == true).FirstOrDefault();
        }

        public bool Update(Invoice model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }
    }
}
