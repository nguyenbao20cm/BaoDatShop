﻿using BaoDatShop.DTO.Invoice;
using BaoDatShop.Responsitories;
using Eshop.Models;
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
        public List<Invoice> GetAllInvoiceOfAccount(string id);
        

    }
    public class InvoiceService : IInvoiceService
    {
        private readonly IProductSizeService IProductSizeService;
        private readonly IInvoiceResponsitories invoiceResponsitories;
        private readonly ICartResponsitories cartResponsitories;
        private readonly IProductResponsitories IProductResponsitories;
        private readonly IProductSizeResponsitories IProductSizeResponsitories;
        private readonly IProductService productService;
        private readonly IInvoiceDetailResponsitories invoiceDetailResponsitories;
        private readonly IProductSizeResponsitories productSizeResponsitories;
        public InvoiceService(IInvoiceResponsitories invoiceResponsitories, ICartResponsitories cartResponsitories, IProductService productService, 
            IInvoiceDetailResponsitories invoiceDetailResponsitories,
            IProductSizeResponsitories productSizeResponsitories, IProductResponsitories IProductResponsitories, 
            IProductSizeService IProductSizeService ,IProductSizeResponsitories IProductSizeResponsitories)
        {
            this.invoiceResponsitories = invoiceResponsitories;
            this.cartResponsitories = cartResponsitories;
            this.productService = productService;
            this.invoiceDetailResponsitories = invoiceDetailResponsitories;
            this.productSizeResponsitories = productSizeResponsitories;
            this.IProductResponsitories = IProductResponsitories;
            this.IProductSizeService = IProductSizeService;
            this.IProductSizeResponsitories = IProductSizeResponsitories;
        }

        public bool Create(string AccountId, CreateInvoiceRequest model)
        {
           
            var Cart = cartResponsitories.GetAll(AccountId);
            foreach (var c in Cart)
            {
                var a = productSizeResponsitories.GetById(c.ProductSizeId);
                if (c.Quantity > a.Stock) return false;
            }
            if (Cart == null) return false;
            Invoice result = new();
            result.Pay = model.Pay;
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
                    var a = productSizeResponsitories.GetById(c.ProductSizeId);
                    if(c.Quantity>a.Stock) return false;
                    a.Stock = a.Stock - c.Quantity;
                    productSizeResponsitories.Update(a);
                    var productId = productSizeResponsitories.GetById(c.ProductSizeId).ProductId;
                    InvoiceDetail detal = new InvoiceDetail
                    {
                        InvoiceId = result.Id,
                        ProductSizeId = c.ProductSizeId,
                        Quantity = c.Quantity,
                        UnitPrice = productService.GetById(productId).Price,
                    };
                    var b=  productService.GetById(productId);
                    b.CountSell= b.CountSell+ c.Quantity;
                    IProductResponsitories.Update(b);
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
                    var a = productSizeResponsitories.GetById(model.ProductSizeID);
                    if (model.Quantity > a.Stock) return false;
                    a.Stock = a.Stock - model.Quantity;
                    productSizeResponsitories.Update(a);
                    var productId = productSizeResponsitories.GetById(model.ProductSizeID).ProductId;
                    InvoiceDetail detal = new InvoiceDetail
                    {
                        InvoiceId = result.Id,
                        ProductSizeId = model.ProductSizeID,
                        Quantity = model.Quantity,
                        UnitPrice = productService.GetById(productId).Price,
                    };
                    var b = productService.GetById(productId);
                    b.CountSell = b.CountSell + model.Quantity;
                    IProductResponsitories.Update(b);
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
            var a= invoiceDetailResponsitories.GetAll(id);
            foreach(var item in a)
            {
                var b = IProductSizeService.GetById(item.ProductSizeId);
                    b.Stock =+ item.Quantity;
                IProductSizeResponsitories.Update(b);
            }
            return invoiceResponsitories.Update(result);
        }

        public bool DeleteInvoiceByCostumer(int id)
        {
            Invoice result = invoiceResponsitories.GetById(id);
            if(result.OrderStatus==1|| result.OrderStatus == 2)
            {
                result.OrderStatus = 4;
                result.Pay = false;
                var a = invoiceDetailResponsitories.GetAll(id);
                foreach (var item in a)
                {
                    var b = IProductSizeService.GetById(item.ProductSizeId);
                    b.Stock = +item.Quantity;
                    IProductSizeResponsitories.Update(b);
                }
                return invoiceResponsitories.Update(result);
            }    
            return false;
        }

        public List<Invoice> GetAll()
        {
            return invoiceResponsitories.GetAll().OrderByDescending(a=>a.IssuedDate).ToList();
        }

        public List<Invoice> GetAllInoviceFilterByDate(string startDate, string endDate)
        {

            return invoiceResponsitories.GetAll().Where(a => a.IssuedDate >= DateTime.Parse(startDate) && a.IssuedDate <= DateTime.Parse(endDate)).ToList();
        }

        public Month GetAllInoviceTotalMonth(string year)
        {
            Month result = new Month();
            var moth1 = invoiceResponsitories.GetAll().Where(a => a.OrderStatus == 5).Where(a=>a.IssuedDate.Year== int.Parse(year)).Where(a => a.IssuedDate.Month < 2).ToList();
            var moth12 = invoiceResponsitories.GetAll().Where(a=>a.OrderStatus==5).Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 13&& a.IssuedDate.Month >11).ToList();
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
            foreach(var a in moth1)
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

        public List<Invoice> GetAllInvoiceOfAccount(string id)
        {
            return invoiceResponsitories.GetAll().Where(a=>a.AccountId==id).ToList();
        }

        public Invoice GetById(int id)
        {
            return invoiceResponsitories.GetById(id);
        }

        public int ProfitForyear(int year)
        {

          var ImportPrice = 0;
          var ImportPiceList=productSizeResponsitories.GetAll().Where(a => a.IssuedDate.Year == year).ToList();
            foreach(var a in ImportPiceList)
            {
                ImportPrice+=a.ImportPrice*a.Stock;
            }
            var Total = 0;
            var TotalList = invoiceResponsitories.GetAll().Where(a=>a.OrderStatus==5).Where(a => a.IssuedDate.Year == year).ToList();
            foreach (var a in TotalList)
            {
                Total += a.Total;
            }
            var b= Total - ImportPrice;
            return Total- ImportPrice;
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
                    if(model.orderStatus==5) result.Pay = true;
            else
            result.Pay = model.pay;
            return invoiceResponsitories.Update(result);
        }
    }
}
