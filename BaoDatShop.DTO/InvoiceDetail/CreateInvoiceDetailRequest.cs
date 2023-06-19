using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.InvoiceDetail
{
    public class CreateInvoiceDetailRequest
    {
        public int InvoiceId { get; set; }

        public int ProductSizeId { get; set; }

        public int Quantity { get; set; } = 1;

        public int UnitPrice { get; set; } = 0;
    }
}
