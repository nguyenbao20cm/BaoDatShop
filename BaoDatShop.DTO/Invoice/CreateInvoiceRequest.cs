using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Invoice
{
    public class CreateInvoiceRequest
    {





        // public string AccountId { get; set; }

        public bool PaymentMethods { get; set; }
        public bool Pay { get; set; }
        public int total { get; set; }
        [DisplayName("Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [DisplayName("SĐT giao hàng")]
        public string ShippingPhone { get; set; }

        //[DisplayName("Tổng tiền")]
        //[DisplayFormat(DataFormatString = "{0:n0}")]
        //[DefaultValue(0)]
    //    public int Total { get; set; } = 0;

        //[DisplayName("Còn hiệu lực")]
        //[DefaultValue(true)]

     //   public bool Pay { get; set; }// Thanh toán
     //   public int OrderStatus { get; set; }//1 chưa xác nhận //2 la chua đang chuẩn bị //3 đang giao//6 đã giao//4 đã hủy,//5hoàn tất

        // Collection reference property cho khóa ngoại từ InvoiceDetail
        
    }
}
