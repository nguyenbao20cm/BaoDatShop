using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class ImportInvoice
    {
        public int Id { get; set; }
        public int ProductSizeId { get; set; }
        public ProductSize ProductSize { get; set; }
      
        public DateTime IssuedDate { get; set; } = DateTime.Now;
        public int ImportPrice { get; set; }
        public int Quantity { get; set; } = 0;
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        
    }
}
