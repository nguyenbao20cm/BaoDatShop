using BaoDatShop.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public int ProductSizeId { get; set; }
        public ProductSize ProductSize { get; set; }
        public int Quantity { get; set; } = 1;
        public int UnitPrice { get; set; } = 0;
    }
}
