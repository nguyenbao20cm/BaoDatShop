using System.ComponentModel;

namespace CodeMegaVNPay.Models;

public class PaymentResponseModel
{
    public string OrderDescription { get; set; }
    public string TransactionId { get; set; }
    public string OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentId { get; set; }
    public bool Success { get; set; }
    public string Token { get; set; }
    public string VnPayResponseCode { get; set; }
    public int Total { get; set; }


    public string CardType { get; set; }//Loại thẻ
    public string CodeBank { get; set; }// mã ngân hàng
    public string InvoiceBankID { get; set; }//mã giao dihc tại ngân hàng
    public string VNBillId { get; set; }// MÃ giao dịch tại VPN PAY
    public string DateTime { get; set; }// TG Thanh toán


    public string NameCustomer { get; set; }

    //public bool PaymentMethods { get; set; }
    //public bool Pay { get; set; }

    //public string ShippingAddress { get; set; }
    //public string ShippingPhone { get; set; }
}