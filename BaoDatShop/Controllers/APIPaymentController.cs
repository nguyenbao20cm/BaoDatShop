using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using CodeMegaVNPay.Services;
using Eshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIPaymentController : ControllerBase
    {
        private readonly IInvoiceDetailResponsitories IInvoiceDetailResponsitories;
        private readonly IProductResponsitories IProductResponsitories;
        private readonly IProductService IProductService;
        private readonly IInvoiceService invoiceService;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext context;
        private readonly IInvoiceResponsitories IInvoiceResponsitories;
        private readonly ICartResponsitories ICartResponsitories;
        private readonly IProductSizeResponsitories IProductSizeResponsitories;
        public APIPaymentController(IInvoiceService invoiceService, ICartResponsitories ICartResponsitories, IInvoiceResponsitories IInvoiceResponsitories,
            AppDbContext context,IConfiguration _configuration,
            IProductSizeResponsitories IProductSizeResponsitories,
            IProductResponsitories IProductResponsitories,
            IInvoiceDetailResponsitories IInvoiceDetailResponsitories,
            IProductService IProductService,
            IVnPayService _vnPayService)
        {
            this.IInvoiceDetailResponsitories = IInvoiceDetailResponsitories;
            this.IProductResponsitories = IProductResponsitories;
            this.IProductService = IProductService;
            this.ICartResponsitories = ICartResponsitories;
            this._configuration = _configuration;
            this.invoiceService = invoiceService;
            this.IProductSizeResponsitories = IProductSizeResponsitories;
            this._vnPayService=  _vnPayService;
            this.IInvoiceResponsitories = IInvoiceResponsitories;
            this.context = context;
        }
        private readonly IVnPayService _vnPayService;
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("CreateVNPAYURL")]
        public async Task<IActionResult> CreatePaymentUrl(CreateInvoiceRequest model)
        {
            var Cart = ICartResponsitories.GetAll(GetCorrectUserId());
            foreach (var c in Cart)
            {
                var a = IProductSizeResponsitories.GetById(c.ProductSizeId);
                if (c.Quantity > a.Stock) return Ok(false);
            }
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(url);

        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpGet("CheckReuslt")]
        public async Task<IActionResult> CheckReuslt()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            return Ok(response);
        }
            [Authorize(Roles = UserRole.Costumer)]
        [HttpGet("GeDATaURL")]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response.VnPayResponseCode == "00")
            {
                var orderDescription = response.OrderDescription.Split('/');
                var paymentmethod = orderDescription[0];
                var total = orderDescription[2];
                var address = orderDescription[3];
                var phone = orderDescription[4];
                var name = orderDescription[5];
                var Idacc = GetCorrectUserId();

                var Cart = ICartResponsitories.GetAll(GetCorrectUserId());
                Invoice result = new();
                result.Pay =true;
                result.NameCustomer = name;
                result.PaymentMethods = bool.Parse(paymentmethod);
                result.AccountId = GetCorrectUserId();
                result.IssuedDate = DateTime.Now;
                result.ShippingAddress = address;
                result.ShippingPhone = phone;
                result.ShippingPhone = phone;
                result.Status = true;
                result.OrderStatus = 1;
                result.Total = int.Parse(total);
                var tamp = IInvoiceResponsitories.Create(result);
                if (tamp == true)
                {
                    VnpayBill a = new();
                    a.DateTime = DateTime.Now;
                    a.Total = response.Total;
                    a.VNBillId = response.PaymentId;
                    a.AccountId = Idacc;
                    a.InvoiceId = result.Id;
                    context.Add(a);
                    context.SaveChanges();
                    foreach (var c in Cart)
                    {
                        var a1 = IProductSizeResponsitories.GetById(c.ProductSizeId);
                        a1.Stock = a1.Stock - c.Quantity;
                        IProductSizeResponsitories.Update(a1);
                        var productId = IProductSizeResponsitories.GetById(c.ProductSizeId).ProductId;
                        InvoiceDetail detal = new InvoiceDetail
                        {
                            InvoiceId = result.Id,
                            ProductSizeId = c.ProductSizeId,
                            Quantity = c.Quantity,
                            UnitPrice = IProductService.GetById(productId).Price,
                        };
                        var b = IProductService.GetById(productId);
                        b.CountSell = b.CountSell + c.Quantity;
                        IProductResponsitories.Update(b);
                        IInvoiceDetailResponsitories.Create(detal);
                        ICartResponsitories.DeleteAll(GetCorrectUserId());
                    }
                }
            }
            return Ok(response);
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("CreateVNPAYURLBuyNow")]
        public async Task<IActionResult> CreatePaymentUrlNow(CreateInvoiceNow model)
        {
            var url = _vnPayService.CreatePaymentUrlBuyNow(model, HttpContext);
            return Ok(url);

        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpGet("GeDATaURLBuyNow")]
        public async Task<IActionResult> PaymentCallbackNow()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.VnPayResponseCode=="00")
            {
                var orderDescription = response.OrderDescription.Split('/');
                var paymentmethod = orderDescription[0];
                var total = orderDescription[2];
                var address = orderDescription[3];
                var phone = orderDescription[4];
                var name = orderDescription[5];
                var productsizeid = orderDescription[6];
                var quanlity = orderDescription[7];
                var Idacc = GetCorrectUserId(); 

                Invoice result = new();
                result.Pay = true;
                result.PaymentMethods = bool.Parse(paymentmethod);
                result.AccountId = GetCorrectUserId();
                result.NameCustomer = name;
                result.IssuedDate = DateTime.Now;
                result.ShippingAddress = address;
                result.ShippingPhone = phone;
                result.Total = int.Parse(total);
                result.Status = true;
                result.OrderStatus = 1;
                var tamp = IInvoiceResponsitories.Create(result);
                if (tamp == true)
                {
                    var a = IProductSizeResponsitories.GetById(int.Parse(productsizeid));
                    IProductSizeResponsitories.Update(a);
                    var productId = IProductSizeResponsitories.GetById(int.Parse(productsizeid)).ProductId;
                    InvoiceDetail detal = new InvoiceDetail
                    {
                        InvoiceId = result.Id,
                        ProductSizeId = int.Parse(productsizeid),
                        Quantity = int.Parse(quanlity),
                        UnitPrice = IProductService.GetById(productId).Price,
                    };
                    var b = IProductService.GetById(productId);
                    b.CountSell = b.CountSell + int.Parse(quanlity);
                    IProductResponsitories.Update(b);
                    IInvoiceDetailResponsitories.Create(detal);

                    VnpayBill a2 = new();
                    a2.DateTime = DateTime.Now;
                    a2.Total = response.Total;
                    a2.VNBillId = response.PaymentId;
                    a2.AccountId = GetCorrectUserId();
                    a2.InvoiceId = result.Id;
                    context.Add(a);
                    context.SaveChanges();
                }
            
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

