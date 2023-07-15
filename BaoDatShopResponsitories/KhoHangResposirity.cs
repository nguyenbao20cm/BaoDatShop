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
    public interface IWarehouseResposirity
    {
        public List<Warehouse> GetAll();
        public Warehouse GetById(int id);
    }
    public class WarehouseResposirity : IWarehouseResposirity
    {
        private readonly AppDbContext context;
        public WarehouseResposirity(AppDbContext context)
        {
            this.context = context;
        }

        public List<Warehouse> GetAll()
        {
            return context.Warehouse.Include(a=>a.ProductSize.Product).Include(a => a.ProductSize).ToList();
        }

        public Warehouse GetById(int id)
        {
            return context.Warehouse.Include(a => a.ProductSize.Product).Include(a => a.ProductSize).Where(a=>a.Id==id).FirstOrDefault();
        }
    }
}
