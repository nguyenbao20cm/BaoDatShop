using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class VnpayBill
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public string VNBillId { get; set; }
        public DateTime DateTime { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
