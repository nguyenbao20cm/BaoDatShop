using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Product;
using BaoDatShop.Responsitories;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BaoDatShop.Service
{
    public interface IInvoiceService
    {
        public bool Create(string AccountId, CreateInvoice model);
        public bool Update(int id, CreateInvoice model);
        public bool Delete(int id);
        public Invoice GetById(int id);
        public List<Invoice> GetAll();
    }
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceResponsitories invoiceResponsitories;
        private readonly ICartResponsitories cartResponsitories;
        private readonly IProductService productService;
        private readonly IInvoiceDetailResponsitories invoiceDetailResponsitories;
        public InvoiceService(IInvoiceResponsitories invoiceResponsitories, ICartResponsitories cartResponsitories, IProductService productService, IInvoiceDetailResponsitories invoiceDetailResponsitories)
        {
            this.invoiceResponsitories = invoiceResponsitories;
            this.cartResponsitories = cartResponsitories;
            this.productService = productService;
            this.invoiceDetailResponsitories = invoiceDetailResponsitories;
        }

        public bool Create(string AccountId, CreateInvoice model)
        {
            var Cart = cartResponsitories.GetAll(AccountId);
            if (Cart == null) return false;
            Invoice result = new();
            result.Code = model.Code;
            result.AccountId = AccountId;
            result.IssuedDate = model.IssuedDate;
            result.ShippingAddress = model.ShippingAddress;
            result.ShippingPhone = model.ShippingPhone;
            int toal = 0;
            foreach (var item in Cart)
            {
                toal += item.Quantity * productService.GetById(item.ProductId).Price;
            }
            result.Total = toal;
            result.Pay = false;
            result.Status = true;
            result.OrderStatus = 1;
            var tamp =invoiceResponsitories.Create(result);
            if (tamp == true)
            {
               
                foreach (var c in Cart)
                {
                    InvoiceDetail detal = new InvoiceDetail
                    {
                        InvoiceId = result.Id,
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                        UnitPrice = productService.GetById(c.ProductId).Price,
                    };
                    invoiceDetailResponsitories.Create(detal);
                    cartResponsitories.DeleteAll(AccountId);
                }
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

        public List<Invoice> GetAll()
        {
            return invoiceResponsitories.GetAll();
        }

        public Invoice GetById(int id)
        {
            return invoiceResponsitories.GetById(id);
        }
        // dang lam
        public bool Update(int id, CreateInvoice model)
        {
            Invoice result = invoiceResponsitories.GetById(id);
            result.Code = model.Code;
            //result.AccountId = AccountId;
            result.IssuedDate = model.IssuedDate;
            result.ShippingAddress = model.ShippingAddress;
            result.ShippingPhone = model.ShippingPhone;
            //result.Total = model.Total;
            //result.Pay = model.Pay;
            //result.OrderStatus = model.OrderStatus;
            return invoiceResponsitories.Update(result);
        }
    }
}
