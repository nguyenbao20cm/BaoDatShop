﻿using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Role;
using BaoDatShop.DTO.Voucher;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using CodeMegaVNPay.Services;
using Eshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Globalization;
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
        private readonly IWarehouseResposirity IWarehouseResposirity;
        private readonly IEmailSender IEmailSender;
      
        public APIPaymentController(IInvoiceService invoiceService, ICartResponsitories ICartResponsitories, IInvoiceResponsitories IInvoiceResponsitories,
            AppDbContext context,IConfiguration _configuration,
            IEmailSender IEmailSender,
            IProductSizeResponsitories IProductSizeResponsitories,
            IProductResponsitories IProductResponsitories,
            IInvoiceDetailResponsitories IInvoiceDetailResponsitories,
            IWarehouseResposirity IWarehouseResposirity,
            IProductService IProductService,
            IVnPayService _vnPayService)
        {
            this.IEmailSender = IEmailSender;
            this.IInvoiceDetailResponsitories = IInvoiceDetailResponsitories;
            this.IProductResponsitories = IProductResponsitories;
            this.IProductService = IProductService;
            this.ICartResponsitories = ICartResponsitories;
            this._configuration = _configuration;
            this.invoiceService = invoiceService;
            this.IWarehouseResposirity = IWarehouseResposirity;
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
                var a = IWarehouseResposirity.GetAll().Where(a=>a.ProductSizeId==c.ProductSizeId).FirstOrDefault();
                if (c.Quantity > a.Stock) return Ok("Sản phẩm không đủ số lượng tồn kho");
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
                var VoucherId = orderDescription[6];
                if (VoucherId == "")
                    VoucherId = null;
                var Idacc = GetCorrectUserId();


                var Cart = ICartResponsitories.GetAll(GetCorrectUserId());
                foreach (var c in Cart)
                {
                    var a = context.Warehouse.Include(a=>a.ProductSize).Where(a=>a.ProductSizeId==c.ProductSizeId).FirstOrDefault();
                    if (c.Quantity > a.Stock) return Ok("Thất bại");
                }
                Invoice result = new();
                result.Pay =true;
                if(VoucherId!=null)
                 result.VoucherId = int.Parse(VoucherId);
                if (VoucherId == null)
                    result.VoucherId = null;
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
                    SendVoucher mail = new();
                    mail.subject = "Xác nhận thanh toán trực tuyến thành công";
                    mail.message = "Cảm ơn bạn đã mua hàng ở "+context.Footer.FirstOrDefault().Title    +".Đơn hàng của bạn thanh toán trực tuyến của bạn đã thành công. Dưới đây là mã hóa đơn giao dịch:#" + result.Id;
                    mail.email = context.Account.Where(a => a.Id == GetCorrectUserId()).FirstOrDefault().Email;
                    IEmailSender.SendEmailPayOnline(mail);
                    VnpayBill a = new();
                    a.DateTime = FormatDate(response.DateTime);
                    a.CodeBank = response.CodeBank;
                    a.VNBillId = response.VNBillId;
                    a.InvoiceBankID = response.InvoiceBankID;
                    a.CardType = response.CardType;
                    a.Total = int.Parse(total);
                    a.VNBillId = response.PaymentId;
                    a.AccountId = Idacc;
                    a.InvoiceId = result.Id;
                    context.Add(a);
                    context.SaveChanges();
                    foreach (var c in Cart)
                    {
                        var a1 = IWarehouseResposirity.GetAll().Where(a=>a.ProductSizeId==c.ProductSizeId).FirstOrDefault();
                        a1.Stock = a1.Stock - c.Quantity;
                        context.Update(a1);
                        context.SaveChanges();
                        var productId = IProductSizeResponsitories.GetById(c.ProductSizeId).ProductId;
                        InvoiceDetail detal = new InvoiceDetail
                        {
                            InvoiceId = result.Id,
                            ProductSizeId = c.ProductSizeId,
                            Quantity = c.Quantity,
                            UnitPrice = IProductService.GetById(productId).PriceSales,
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
            var Cart = ICartResponsitories.GetAll(GetCorrectUserId());
            foreach (var c in Cart)
            {
                var a = IWarehouseResposirity.GetAll().Where(a => a.ProductSizeId == c.ProductSizeId).FirstOrDefault();
                if (c.Quantity > a.Stock) return Ok("Sản phẩm không đủ số lượng tồn kho");
            }
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
                var productsizeid = orderDescription[7];
                var quanlity = orderDescription[6];
                var VoucherId = orderDescription[8];
                if (VoucherId == "")
                    VoucherId = null;
                var Idacc = GetCorrectUserId();
                var ab = IWarehouseResposirity.GetAll().Where(a=>a.ProductSizeId==int.Parse(productsizeid)).FirstOrDefault();
                if (int.Parse(quanlity) > ab.Stock) return Ok("thất bại");
                Invoice result = new();
                result.Pay = true;
                if (VoucherId != null)
                    result.VoucherId = int.Parse(VoucherId);
                if (VoucherId == null)
                    result.VoucherId = null;
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
                    SendVoucher mail = new();
                    mail.subject = "Xác nhận thanh toán trực tuyến thành công";
                    mail.message = "Chúng tôi xin thông báo rằng thanh toán trực tuyến của bạn đã thành công. Dưới đây là mã hóa đơn giao dịch:#" + result.Id;
                    mail.email = context.Account.Where(a => a.Id == GetCorrectUserId()).FirstOrDefault().Email;
                    IEmailSender.SendEmailPayOnline(mail);
                    var a = IWarehouseResposirity.GetAll().Where(a => a.ProductSizeId == int.Parse(productsizeid)).FirstOrDefault();
                    a.Stock = a.Stock - int.Parse(quanlity);
                    context.Update(a);
                    context.SaveChanges();
                    var productId = IProductSizeResponsitories.GetById(int.Parse(productsizeid)).ProductId;
                    InvoiceDetail detal = new InvoiceDetail
                    {
                        InvoiceId = result.Id,
                        ProductSizeId = int.Parse(productsizeid),
                        Quantity = int.Parse(quanlity),
                        UnitPrice = IProductService.GetById(productId).PriceSales,
                    };
                    var b = IProductService.GetById(productId);
                    b.CountSell = b.CountSell + int.Parse(quanlity);
                    IProductResponsitories.Update(b);
                    IInvoiceDetailResponsitories.Create(detal);

                    VnpayBill a2 = new();
                    a2.DateTime = DateTime.Now;
                    a2.CodeBank = response.CodeBank;
                    a2.VNBillId = response.VNBillId;
                    a2.InvoiceBankID = response.InvoiceBankID;
                    a2.CardType = response.CardType;
                    a2.Total = int.Parse(total);
                    //a2.VNBillId = response.PaymentId;
                    a2.AccountId = GetCorrectUserId();
                    a2.InvoiceId = result.Id;
                    context.Add(a2);
                    context.SaveChanges();
                }
            
            }
            return Ok(response);
        }
        private DateTime FormatDate(string e)
        {
            string dateString = e;
            string format = "yyyyMMddHHmmss";
            DateTime result = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
            return result;

        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}

