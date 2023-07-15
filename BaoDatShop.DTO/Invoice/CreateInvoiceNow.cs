using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Invoice
{
    public class CreateInvoiceNow
    {
        public string NameCustomer { get; set; }
        public int Quantity { get; set; }
        public int ProductSizeID { get; set; }
        public bool PaymentMethods { get; set; }
        public bool Pay { get; set; }
        public int total { get; set; }
        public int VoucherId { get; set; }

        public string ShippingAddress { get; set; }

  
        public string ShippingPhone { get; set; }
    }
}
