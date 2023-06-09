﻿using BaoDatShop.DTO.InvoiceDetail;
using BaoDatShop.DTO.Product;
using BaoDatShop.Responsitories;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IInvoiceDetailService
    {
        public bool Create(CreateInvoiceDetailRequest model);
        //public bool Update(int id, CreateInvoiceDetail model);
        //public bool Delete(int id);
        //public InvoiceDetail GetById(int id);
        public List<InvoiceDetail> GetAll(int id);

        public List<InvoiceDetail> GetAll();
    }
    public class InvoiceDetailService : IInvoiceDetailService
    {
        private readonly IInvoiceDetailResponsitories invoiceDetailResponsitories;
        public InvoiceDetailService(IInvoiceDetailResponsitories invoiceDetailResponsitories)
        {
            this.invoiceDetailResponsitories = invoiceDetailResponsitories;
        }
        public bool Create(CreateInvoiceDetailRequest model)
        {
            InvoiceDetail result = new();
            result.ProductSizeId = model.ProductSizeId;
            result.UnitPrice = model.UnitPrice;
            result.Quantity = model.Quantity;
            result.InvoiceId = model.InvoiceId;
            return invoiceDetailResponsitories.Create(result);
        }
        public List<InvoiceDetail> GetAll(int id)
        {
           return invoiceDetailResponsitories.GetAll(id);
        }

        public List<InvoiceDetail> GetAll()
        {
            return invoiceDetailResponsitories.GetAll();
        }
    }
}
