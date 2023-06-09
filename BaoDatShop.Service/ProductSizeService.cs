﻿using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.News;
using BaoDatShop.DTO.ProductSize;
using BaoDatShop.DTO.ProductType;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IProductSizeService
    {
        public bool Create(string id,CreateProductSize model);
        public bool Update(int id, string idacc, UpdateProductSize model);
        public bool Delete(int id);
        public ProductSize GetById(int id);
        public List<ProductSize> GetAll();
        public List<ProductSize> GetAllProductTypeStatusTrue();
        public List<ProductSize> GetAllProductTypeStatusFalse();
        public Month GetAllImportPrice(string year);
        
        public List<ProductSize> GetProductSizeById(int id);
        public int GetProductIdByProductSize(int id);
        
    }
    public class ProductSizeService: IProductSizeService
    {
        private readonly IProductSizeResponsitories IProductSizeResponsitories;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        private readonly IImportInvoiceResponsitories IImportInvoiceResponsitories;
        private readonly AppDbContext context;
        public ProductSizeService(IImportInvoiceResponsitories IImportInvoiceResponsitories,
            AppDbContext context,
            IProductSizeResponsitories IProductSizeResponsitories, IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.IProductSizeResponsitories = IProductSizeResponsitories;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
            this.IImportInvoiceResponsitories = IImportInvoiceResponsitories;
            this.context = context;
        }
        public List<ProductSize> GetAllProductTypeStatusFalse()
        {
            return IProductSizeResponsitories.GetAll().Where(a => a.Status == false).ToList();
        }

        public List<ProductSize> GetAllProductTypeStatusTrue()
        {
            return IProductSizeResponsitories.GetAll().Where(a => a.Status == true).ToList();
        }
        public bool Create(string id,CreateProductSize model)
        {
            ProductSize result = new();
            result.Name = model.Name;
            //result.ImportPrice = model.ImportPrice;
            //result.IssuedDate = DateTime.Now;
            //result.SupplierId = model.SupplierId;
            result.ProductId = model.ProductId;
            result.Status = model.Status;
            //result.Stock = model.Stock;
            
            var ab = IProductSizeResponsitories.Create(result);
            if (ab==true)
            {
                Warehouse kho = new();
                kho.ProductSizeId = result.Id;
                kho.Stock = 0;
                context.Add(kho);
                context.SaveChanges();
               var tam= IProductSizeResponsitories.GetById(result.Id);
                HistoryAccount a = new();
                a.AccountID = id; a.Datetime = DateTime.Now;
                a.Content = "Đã thêm Size " + model.Name + " cho sản phẩm " + tam.Product.Name;
                IHistoryAccountResponsitories.Create(a);
            }    
            return ab;
        }

        public bool Delete(int id)
        {
            ProductSize result = IProductSizeResponsitories.GetById(id);
            result.Status = false;  
            return IProductSizeResponsitories.Update(result);
        }

        public List<ProductSize> GetAll()
        {
            return IProductSizeResponsitories.GetAll();
        }



        public ProductSize GetById(int id)
        {
            var reslut = IProductSizeResponsitories.GetById(id);
     
            return reslut;
        }

        public bool Update(int id,string idacc, UpdateProductSize model)
        {
            ProductSize result = IProductSizeResponsitories.GetById(id);
            result.Name = model.Name;
            //result.ImportPrice = model.ImportPrice;
            result.Status = model.Status;
            //result.Stock = model.Stock;
            result.ProductId = model.ProductId;
            //result.SupplierId = model.SupplierId;
            var ab = IProductSizeResponsitories.Update(result);
            if (ab == true)
            {
                var tam = IProductSizeResponsitories.GetById(result.Id);
                HistoryAccount a = new();
                a.AccountID = idacc;
                a.Content = "Đã cập nhật dữ liệu cho cho sản phẩm " + tam.Product.Name;
                a.Datetime = DateTime.Now;
                IHistoryAccountResponsitories.Create(a);
            }
            return ab;
        }

        public Month GetAllImportPrice(string year)
        {
       
            Month result = new Month();
            var moth1 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 2).ToList();
            var moth12 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 13 && a.IssuedDate.Month > 11).ToList();
            var moth2 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 3 && a.IssuedDate.Month > 1).ToList();
            var moth3 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 4 && a.IssuedDate.Month > 2).ToList();
            var moth4 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 5 && a.IssuedDate.Month > 3).ToList();
            var moth5 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 6 && a.IssuedDate.Month > 4).ToList();
            var moth6 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 7 && a.IssuedDate.Month > 5).ToList();
            var moth7 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 8 && a.IssuedDate.Month > 6).ToList();
            var moth8 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 9 && a.IssuedDate.Month > 7).ToList();
            var moth9 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 10 && a.IssuedDate.Month > 8).ToList();
            var moth10 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 11 && a.IssuedDate.Month > 9).ToList();
            var moth11 = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Year == int.Parse(year)).Where(a => a.IssuedDate.Month < 12 && a.IssuedDate.Month > 10).ToList();
            var total1 = 0;
            foreach (var a in moth1)
            {
                total1 += a.ImportPrice*a.Quantity;
            }
            result.Month1 = total1;
            var total2 = 0;
            foreach (var a in moth2)
            {
                total2 += a.ImportPrice * a.Quantity;
            }
            result.Month2 = total2;
            var total3 = 0;
            foreach (var a in moth3)
            {
                total3 += a.ImportPrice * a.Quantity;
            }
            result.Month3 = total3;
            var total4 = 0;
            foreach (var a in moth4)
            {
                total4 += a.ImportPrice * a.Quantity;
            }
            result.Month4 = total4;
            var total5 = 0;
            foreach (var a in moth5)
            {
                total5 += a.ImportPrice * a.Quantity;
            }
            result.Month5 = total5; var total6 = 0;
            foreach (var a in moth6)
            {
                total6 += a.ImportPrice * a.Quantity;
            }
            result.Month6 = total6; var total7 = 0;
            foreach (var a in moth7)
            {
                total7 += a.ImportPrice * a.Quantity;
            }
            result.Month7 = total7; var total8 = 0;
            foreach (var a in moth8)
            {
                total8 += a.ImportPrice * a.Quantity;
            }
            result.Month8 = total8; var total9 = 0;
            foreach (var a in moth9)
            {
                total9 += a.ImportPrice * a.Quantity;
            }
            result.Month9 = total9; var total10 = 0;
            foreach (var a in moth10)
            {
                total10 += a.ImportPrice * a.Quantity;
            }
            result.Month10 = total10; var total11 = 0;
            foreach (var a in moth11)
            {
                total11 += a.ImportPrice * a.Quantity;
            }
            result.Month11 = total11; var total12 = 0;
            foreach (var a in moth12)
            {
                total12 += a.ImportPrice * a.Quantity;
            }
            result.Month12 = total12;
            return result;
        }

        public List<ProductSize> GetProductSizeById(int id)
        {
            return IProductSizeResponsitories.GetAll().Where(a=>a.Status==true).Where(a=>a.ProductId==id).ToList();
        }

        public int GetProductIdByProductSize(int id)
        {
            return IProductSizeResponsitories.GetById(id).ProductId;
        }
    }
}
