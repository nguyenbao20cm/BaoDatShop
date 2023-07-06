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


    public string NameCustomer { get; set; }
    public string Time { get; set; }
    //public bool PaymentMethods { get; set; }
    //public bool Pay { get; set; }
    //public int Total { get; set; }
    //public string ShippingAddress { get; set; }
    //public string ShippingPhone { get; set; }
}