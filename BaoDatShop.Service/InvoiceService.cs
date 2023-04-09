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

namespace BaoDatShop.Service
{
    public interface IInvoiceService
    {
        public bool Create(CreateInvoice model);
        public bool Update(int id, CreateInvoice model);
        public bool Delete(int id);
        public Invoice GetById(int id);
        public List<Invoice> GetAll();
    }
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceResponsitories invoiceResponsitories;
        public InvoiceService(IInvoiceResponsitories invoiceResponsitories)
        {
            this.invoiceResponsitories = invoiceResponsitories;
        }

        public bool Create(CreateInvoice model)
        {
            Invoice result = new();
            result.Code = model.Code;
            result.AccountId = model.AccountId;
            result.IssuedDate = model.IssuedDate;
            result.ShippingAddress = model.ShippingAddress;
            result.ShippingPhone = model.ShippingPhone;
            result.Total = model.Total;
            result.Pay = model.Pay; result.Status = true;
            result.OrderStatus = model.OrderStatus;
            return invoiceResponsitories.Create(result);
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

        public bool Update(int id, CreateInvoice model)
        {
            Invoice result = invoiceResponsitories.GetById(id);
            result.Code = model.Code;
            result.AccountId = model.AccountId;
            result.IssuedDate = model.IssuedDate;
            result.ShippingAddress = model.ShippingAddress;
            result.ShippingPhone = model.ShippingPhone;
            result.Total = model.Total;
            result.Pay = model.Pay;
            result.OrderStatus = model.OrderStatus;
            return invoiceResponsitories.Update(result);
        }
    }
}
