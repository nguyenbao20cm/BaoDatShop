using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //public DateTime IssuedDate { get; set; } = DateTime.Now;
        //public int ImportPrice { get; set; }
        public int Stock { get; set; } 

        //public int SupplierId { get; set; }
        //public Supplier Supplier { get; set; }
        public bool Status { get; set; }

    }
}
