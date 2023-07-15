using Eshop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaoDatShop.Model.Model;

namespace BaoDatShop.Model.DTO.Invoice
{
    public class GetInvoiceForCustomer
    {
        public int GiaCu { get; set; }
        public int Id { get; set; }
        public int? VoucherId { get; set; }
        public Voucher Voucher { get; set; }

        public string NameCustomer { get; set; }
        public bool PaymentMethods { get; set; }// true là COD flase la lay hang tai shop
        public string AccountId { get; set; }

        // Navigation reference property cho khóa ngoại đến Account 
        [DisplayName("Khách hàng")]
        public Account Account { get; set; }

        [DisplayName("Ngày lập")]
        [DataType(DataType.DateTime)]
        public DateTime IssuedDate { get; set; } = DateTime.Now;

        [DisplayName("Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [DisplayName("SĐT giao hàng")]
        public string ShippingPhone { get; set; }

        [DisplayName("Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        [DefaultValue(0)]
        public int Total { get; set; } = 0;
        [DisplayName("Còn hiệu lực")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
        public bool Pay { get; set; }// Thanh toán   true la
        public int OrderStatus { get; set; }//1 chưa xác nhận //2 la chua đang chuẩn bị //3 đang giao//6 đã giao//4 đã hủy,//5hoàn tất

        // Collection reference property cho khóa ngoại từ InvoiceDetail
        //public List<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
