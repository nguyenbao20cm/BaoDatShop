
using BaoDatShop.DTO;
using BaoDatShop.DTO.Invoice;
using CodeMegaVNPay.Models;
using Microsoft.AspNetCore.Http;

namespace CodeMegaVNPay.Services;
public interface IVnPayService
{
    string CreatePaymentUrl(CreateInvoiceRequest model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}