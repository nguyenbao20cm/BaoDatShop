
using BaoDatShop.DTO;
using BaoDatShop.DTO.Invoice;
using BaoDatShop.Model.Model;
using CodeMegaVNPay.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CodeMegaVNPay.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreatePaymentUrl(CreateInvoiceRequest model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.total * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.PaymentMethods}/{model.Pay}/{model.total}/{model.ShippingAddress}/{model.ShippingPhone}/{model.NameCustomer}");
            pay.AddRequestData("vnp_OrderType", model.NameCustomer);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);
            //

            //pay.AddRequestData("vnp_PaymentMethods", model.PaymentMethods.ToString());
            //pay.AddRequestData("vnp_Pay", model.Pay.ToString());
            //pay.AddRequestData("vnp_total", model.total.ToString());
            //pay.AddRequestData("vnp_ShippingPhone", model.ShippingPhone);
            //pay.AddRequestData("vnp_ShippingAddress", model.ShippingAddress);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }
        public string CreatePaymentUrlBuyNow(CreateInvoiceNow model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrlBuyNow"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.total * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.PaymentMethods}/{model.Pay}/{model.total}/{model.ShippingAddress}/{model.ShippingPhone}/{model.NameCustomer}/{model.Quantity}/{model.ProductSizeID}");
            pay.AddRequestData("vnp_OrderType", model.NameCustomer);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);
            //
            //PaymentMethods  Pay total ShippingAddress ShippingPhone namecustomer productsize id so luong
            //pay.AddRequestData("vnp_PaymentMethods", model.PaymentMethods.ToString());
            //pay.AddRequestData("vnp_Pay", model.Pay.ToString());
            //pay.AddRequestData("vnp_total", model.total.ToString());
            //pay.AddRequestData("vnp_ShippingPhone", model.ShippingPhone);
            //pay.AddRequestData("vnp_ShippingAddress", model.ShippingAddress);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }
        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return response;
        }
    }
}
