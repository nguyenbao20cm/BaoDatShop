using BaoDatShop.DTO;
using CodeMegaVNPay.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CodeMegaVNPay.Services;
using BaoDatShop.Model.Context;
using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using BaoDatShop.Model.Model;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIPaymentController : ControllerBase
    {
        private readonly IInvoiceService invoiceService;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext context;
        public APIPaymentController(IInvoiceService invoiceService,
            AppDbContext context,IConfiguration _configuration, IVnPayService _vnPayService)
        {
            this._configuration = _configuration;
            this.invoiceService = invoiceService;
            this._vnPayService=  _vnPayService;
            this.context = context;
        }
        private readonly IVnPayService _vnPayService;
        //[Authorize(Roles = UserRole.Costumer)]
        [HttpPost("CreateVNPAYURL")]
        public async Task<IActionResult> CreatePaymentUrl(CreateInvoiceRequest model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(url);

        }
        //[Authorize(Roles = UserRole.Costumer)]
        [HttpGet("GeDATaURL")]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
       
            if (response.Success)
            {
                VnpayBill a = new();
                a.DateTime = DateTime.Now;
                a.Total = response.Total;
                a.VNBillId = response.PaymentId;
                context.Add(a);
                context.SaveChanges();
            }
            return Ok(response);
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}

