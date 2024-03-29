﻿using BaoDatShop.DTO.Invoice;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.DTO.Invoice;
using BaoDatShop.Responsitories;
using Eshop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace BaoDatShop.Service
{
    public interface IInvoiceService
    {
        public bool DeleteInvoiceByCostumer(int id);
        public bool DeleteInvoice(int id);
        public bool CreateInvoiceNow(string AccountId, CreateInvoiceNow model);
        public bool Create(string AccountId, CreateInvoiceRequest model);
        public bool Update(int id, CreateInvoiceRequest model);
        public bool Delete(int id);
        public Invoice GetById(int id);
        public List<Invoice> GetAll();
        public Month GetAllInoviceTotalMonth(string year);
        public List<Invoice> GetAllInoviceFilterByDate(string startDate, string endDate);
        public bool UpdateInovice(int id, UpdateInvoice model);
        //     public bool UpdateInoviceDelivering(int id);
        public int ProfitForyear(int year);
        public List<GetInvoiceForCustomer> GetAllInvoiceOfAccount(string id);


    }
    public class InvoiceService : IInvoiceService
    {
        private readonly IImportInvoiceResponsitories IImportInvoiceResponsitories;
        private readonly IProductSizeService IProductSizeService;
        private readonly IInvoiceResponsitories invoiceResponsitories;
        private readonly ICartResponsitories cartResponsitories;
        private readonly IProductResponsitories IProductResponsitories;
        private readonly IProductSizeResponsitories IProductSizeResponsitories;
        private readonly IProductService productService;
        private readonly IInvoiceDetailResponsitories invoiceDetailResponsitories;
        private readonly IWarehouseResposirity IWarehouseResposirity;
        private readonly IProductSizeResponsitories productSizeResponsitories;
        private readonly AppDbContext context;
        public InvoiceService(IImportInvoiceResponsitories IImportInvoiceResponsitories, IInvoiceResponsitories invoiceResponsitories, ICartResponsitories cartResponsitories, IProductService productService,
            IInvoiceDetailResponsitories invoiceDetailResponsitories,
            IWarehouseResposirity IWarehouseResposirity,
            AppDbContext context,
            IProductSizeResponsitories productSizeResponsitories, IProductResponsitories IProductResponsitories,
            IProductSizeService IProductSizeService, IProductSizeResponsitories IProductSizeResponsitories)
        {
            this.IImportInvoiceResponsitories = IImportInvoiceResponsitories;
            this.invoiceResponsitories = invoiceResponsitories;
            this.cartResponsitories = cartResponsitories;
            this.productService = productService;
            this.invoiceDetailResponsitories = invoiceDetailResponsitories;
            this.productSizeResponsitories = productSizeResponsitories;
            this.IProductResponsitories = IProductResponsitories;
            this.IProductSizeService = IProductSizeService;
            this.IProductSizeResponsitories = IProductSizeResponsitories;
            this.IWarehouseResposirity = IWarehouseResposirity;
            this.context = context;
        }

        public bool Create(string AccountId, CreateInvoiceRequest model)
        {
            var Cart = cartResponsitories.GetAll(AccountId);
            foreach (var c in Cart)
            {
                var a = IWarehouseResposirity.GetAll().Where(a => a.ProductSizeId == c.ProductSizeId).FirstOrDefault();
                if (c.Quantity > a.Stock) return false;
            }
            if (Cart == null) return false;
            Invoice result = new();
            result.Pay = model.Pay;
            result.VoucherId = model.VoucherId;
            result.NameCustomer = model.NameCustomer;
            result.PaymentMethods = model.PaymentMethods;
            result.AccountId = AccountId;
            result.IssuedDate = DateTime.Now;
            result.ShippingAddress = model.ShippingAddress;
            result.ShippingPhone = model.ShippingPhone;
            int toal = model.total;
            //foreach (var item in Cart)
            //{
            //    var productId = productSizeResponsitories.GetById(item.ProductSizeId).ProductId;
            //    toal += item.Quantity * productService.GetById(productId).Price;
            //}
            result.Total = toal;
            result.Status = true;
            result.OrderStatus = 1;
            var tamp = invoiceResponsitories.Create(result);
            if (tamp == true)
            {
                foreach (var c in Cart)
                {
                    var a = IWarehouseResposirity.GetAll().Where(a => a.ProductSizeId == c.ProductSizeId).FirstOrDefault();
                    if (c.Quantity > a.Stock) return false;
                    a.Stock = a.Stock - c.Quantity;
                    context.Update(a);
                    context.SaveChanges();
                    var productId = productSizeResponsitories.GetById(c.ProductSizeId).ProductId;
                    InvoiceDetail detal = new InvoiceDetail
                    {
                        InvoiceId = result.Id,
                        ProductSizeId = c.ProductSizeId,
                        Quantity = c.Quantity,
                        UnitPrice = productService.GetById(productId).PriceSales,
                    };
                    //var b=  productService.GetById(productId);
                    //b.CountSell= b.CountSell+ c.Quantity;
                    //IProductResponsitories.Update(b);
                    invoiceDetailResponsitories.Create(detal);
                    cartResponsitories.DeleteAll(AccountId);
                }
                return true;
            }
            else return false;

        }

        public bool CreateInvoiceNow(string AccountId, CreateInvoiceNow model)
        {

            Invoice result = new();
            result.Pay = model.Pay;
            result.VoucherId = model.VoucherId;
            result.PaymentMethods = model.PaymentMethods;
            result.AccountId = AccountId;
            result.NameCustomer = model.NameCustomer;
            result.IssuedDate = DateTime.Now;
            result.ShippingAddress = model.ShippingAddress;
            result.ShippingPhone = model.ShippingPhone;
            int toal = model.total;
            result.Total = toal;
            result.Status = true;
            result.OrderStatus = 1;
            var tamp = invoiceResponsitories.Create(result);
            if (tamp == true)
            {
                var a = IWarehouseResposirity.GetAll().Where(a => a.ProductSizeId == model.ProductSizeID).FirstOrDefault();
                if (model.Quantity > a.Stock) return false;
                a.Stock = a.Stock - model.Quantity;
                context.Update(a);
                context.SaveChanges();
                var productId = productSizeResponsitories.GetById(model.ProductSizeID).ProductId;
                InvoiceDetail detal = new InvoiceDetail
                {
                    InvoiceId = result.Id,
                    ProductSizeId = model.ProductSizeID,
                    Quantity = model.Quantity,
                    UnitPrice = productService.GetById(productId).PriceSales,
                };
                //var b = productService.GetById(productId);
                //b.CountSell = b.CountSell + model.Quantity;
                //IProductResponsitories.Update(b);
                invoiceDetailResponsitories.Create(detal);
                return true;
            }
            else return false;
        }

        public bool Delete(int id)
        {
            Invoice result = invoiceResponsitories.GetById(id);
            result.Status = false;

            return invoiceResponsitories.Update(result);
        }

        public bool DeleteInvoice(int id)
        {
            Invoice result = invoiceResponsitories.GetById(id);
            result.OrderStatus = 4;
            result.Pay = false;
            var a = invoiceDetailResponsitories.GetAll(id);
            foreach (var item in a)
            {
                var b = IWarehouseResposirity.GetAll().Where(a => a.ProductSizeId == item.ProductSizeId).FirstOrDefault();
                b.Stock = +item.Quantity;
                context.Update(b);
                context.SaveChanges();
            }
            return invoiceResponsitories.Update(result);
        }

        public bool DeleteInvoiceByCostumer(int id)
        {
            Invoice result = invoiceResponsitories.GetById(id);
            if (result.OrderStatus == 1 || result.OrderStatus == 2)
            {
                result.OrderStatus = 4;
                var a = invoiceDetailResponsitories.GetAll(id);
                foreach (var item in a)
                {
                    var b = IWarehouseResposirity.GetAll().Where(a => a.ProductSizeId == item.ProductSizeId).FirstOrDefault();
                    b.Stock +=item.Quantity;
                    context.Update(b);
                    context.SaveChanges();
                }
                return invoiceResponsitories.Update(result);
            }
            return false;
        }

        public List<Invoice> GetAll()
        {
            return invoiceResponsitories.GetAll().OrderByDescending(a => a.IssuedDate).ToList();
        }

        public List<Invoice> GetAllInoviceFilterByDate(string startDate, string endDate)
        {

            return invoiceResponsitories.GetAll().Where(a => a.IssuedDate.Date >= DateTime.Parse(startDate) && a.IssuedDate.Date <= DateTime.Parse(endDate)).ToList();
        }

        public Month GetAllInoviceTotalMonth(string year)
        {
            Month result = new Month();
            var moth1 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 2).ToList();
            var moth12 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 13 && a.IssuedDate.Month > 11).ToList();
            var moth2 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 3 && a.IssuedDate.Month > 1).ToList();
            var moth3 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 4 && a.IssuedDate.Month > 2).ToList();
            var moth4 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 5 && a.IssuedDate.Month > 3).ToList();
            var moth5 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 6 && a.IssuedDate.Month > 4).ToList();
            var moth6 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 7 && a.IssuedDate.Month > 5).ToList();
            var moth7 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 8 && a.IssuedDate.Month > 6).ToList();
            var moth8 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 9 && a.IssuedDate.Month > 7).ToList();
            var moth9 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 10 && a.IssuedDate.Month > 8).ToList();
            var moth10 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 11 && a.IssuedDate.Month > 9).ToList();
            var moth11 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 12 && a.IssuedDate.Month > 10).ToList();
            var total1 = 0;
            foreach (var a in moth1)
            {
                total1 += a.Total;
            }
            result.Month1 = total1;
            var total2 = 0;
            foreach (var a in moth2)
            {
                total2 += a.Total;
            }
            result.Month2 = total2;
            var total3 = 0;
            foreach (var a in moth3)
            {
                total3 += a.Total;
            }
            result.Month3 = total3;
            var total4 = 0;
            foreach (var a in moth4)
            {
                total4 += a.Total;
            }
            result.Month4 = total4;
            var total5 = 0;
            foreach (var a in moth5)
            {
                total5 += a.Total;
            }
            result.Month5 = total5; var total6 = 0;
            foreach (var a in moth6)
            {
                total6 += a.Total;
            }
            result.Month6 = total6; var total7 = 0;
            foreach (var a in moth7)
            {
                total7 += a.Total;
            }
            result.Month7 = total7; var total8 = 0;
            foreach (var a in moth8)
            {
                total8 += a.Total;
            }
            result.Month8 = total8; var total9 = 0;
            foreach (var a in moth9)
            {
                total9 += a.Total;
            }
            result.Month9 = total9; var total10 = 0;
            foreach (var a in moth10)
            {
                total10 += a.Total;
            }
            result.Month10 = total10; var total11 = 0;
            foreach (var a in moth11)
            {
                total11 += a.Total;
            }
            result.Month11 = total11; var total12 = 0;
            foreach (var a in moth12)
            {
                total12 += a.Total;
            }
            result.Month12 = total12;
            return result;

        }

        public List<GetInvoiceForCustomer> GetAllInvoiceOfAccount(string id)
        {
            List<GetInvoiceForCustomer> result = new();
            var tam = invoiceResponsitories.GetAll().Where(a => a.AccountId == id).ToList();
            foreach (var a in tam)
            {
                GetInvoiceForCustomer tam1 = new();
                tam1.Total = a.Total;
                tam1.VoucherId = a.VoucherId;
                tam1.Id = a.Id;
                tam1.AccountId = a.AccountId;
                tam1.PaymentMethods = a.PaymentMethods;
                tam1.Account = a.Account;
                tam1.NameCustomer = a.NameCustomer;
                tam1.Voucher = a.Voucher;
                tam1.IssuedDate = a.IssuedDate;
                tam1.ShippingAddress = a.ShippingAddress;
                tam1.ShippingPhone = a.ShippingPhone;
                tam1.Pay = a.Pay;
                tam1.OrderStatus = a.OrderStatus;
                var giacu = 0;
                var a1= context.InvoiceDetail.Include(a => a.ProductSize).Include(a => a.ProductSize.Product).Where(a => a.InvoiceId == tam1.Id).ToList();
                foreach(var item in a1)
                {
                    giacu += item.ProductSize.Product.PriceSales * item.Quantity;
                }
                tam1.GiaCu = giacu;
                result.Add(tam1);
            }    
            return result;
        }

        public Invoice GetById(int id)
        {
            return invoiceResponsitories.GetById(id);
        }

        public int ProfitForyear(int year)
        {

            var ImportPrice = 0;
            var ImportPiceList = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == year).ToList();
            foreach (var a in ImportPiceList)
            {
                ImportPrice += a.ImportPrice * a.Quantity;
            }
            var Total = 0;
            var TotalList = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == year).ToList();
            foreach (var a in TotalList)
            {
                Total += a.Total;
            }
            var Ship = 0;
            var ShipList = invoiceResponsitories.GetAll().Where(a => a.PaymentMethods == true).Where(a => a.OrderStatus == 3 || a.OrderStatus == 5)
                .Where(a => a.IssuedDate.Year == year).ToList();
            foreach (var a in ShipList)
            {
                Ship += +1;
            }
            var b = Total - ImportPrice- Ship*35000;
            return b;
        }

        // dang lam
        public bool Update(int id, CreateInvoiceRequest model)
        {
            Invoice result = invoiceResponsitories.GetById(id);

            //result.AccountId = AccountId;
            result.IssuedDate = DateTime.Now;
            result.ShippingAddress = model.ShippingAddress;
            result.ShippingPhone = model.ShippingPhone;
            //result.Total = model.Total;
            //result.Pay = model.Pay;
            //result.OrderStatus = model.OrderStatus;
            return invoiceResponsitories.Update(result);
        }

        public bool UpdateInovice(int id, UpdateInvoice model)
        {
            Invoice result = invoiceResponsitories.GetById(id);
            result.OrderStatus = model.orderStatus;

            result.ShippingAddress = model.shippingadress;
            result.ShippingPhone = model.shippingphone;
            if (model.orderStatus == 5) result.Pay = true;
            //else
            //result.Pay = model.pay;
            return invoiceResponsitories.Update(result);
        }
    }
}
