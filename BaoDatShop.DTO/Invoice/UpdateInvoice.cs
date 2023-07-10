using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Invoice
{
    public class UpdateInvoice
    {
        public string shippingphone { get; set; }
        public string shippingadress { get; set; }
        //public bool pay { get; set; }
        public int orderStatus { get; set; }
    }
}
