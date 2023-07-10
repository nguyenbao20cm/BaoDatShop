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

        public string CardType { get; set; }//Loại thẻ
        public string CodeBank { get; set; }// mã ngân hàng
        public int Total { get; set; }
        public string InvoiceBankID { get; set; }//mã giao dihc tại ngân hàng
        public string VNBillId { get; set; }// MÃ giao dịch tại VPN PAY
        public DateTime DateTime { get; set; }// TG Thanh toán


        public string AccountId { get; set; }
        public Account Account { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
