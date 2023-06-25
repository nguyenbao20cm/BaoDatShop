using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Responsitories
{
    public interface IHistoryAccountResponsitories
    {
        public List<HistoryAccount> GetAll();
        public HistoryAccount GetById(int id);
        public bool Update(HistoryAccount model);
        public bool Create(HistoryAccount model);
    }
    public class HistoryAccountResponsitories: IHistoryAccountResponsitories
    {
        private readonly AppDbContext context;
        public HistoryAccountResponsitories(AppDbContext context)
        {
            this.context = context;
        }
        public bool Create(HistoryAccount model)
        {
            context.Add(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public List<HistoryAccount> GetAll()
        {
            if (context.HistoryAccount.ToList() == null) return null;
            return context.HistoryAccount.ToList();
        }

        public bool Update(HistoryAccount model)
        {
            context.Update(model);
            int check = context.SaveChanges();
            return check > 0 ? true : false;
        }

        public HistoryAccount GetById(int id)
        {
            if (context.HistoryAccount.Where(a => a.Id == id).FirstOrDefault() == null) return null;
            return context.HistoryAccount.Where(a => a.Id == id).FirstOrDefault();
        }
    }
}
