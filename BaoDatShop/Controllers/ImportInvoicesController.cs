using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportInvoicesController : ControllerBase
    {
        private readonly IConverter _convert;
        private readonly AppDbContext context;
        private readonly IImportInvoiceResponsitories IImportInvoiceResponsitories;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        private readonly IProductSizeResponsitories IProductSizeResponsitories;
        public ImportInvoicesController(IHistoryAccountResponsitories IHistoryAccountResponsitories,
            IConverter _convert,
            IProductSizeResponsitories IProductSizeResponsitories,
            IImportInvoiceResponsitories IImportInvoiceResponsitories, AppDbContext context)
        {
            this.context = context;
            this._convert = _convert;
            this.IProductSizeResponsitories = IProductSizeResponsitories;
            this.IImportInvoiceResponsitories = IImportInvoiceResponsitories;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
        }
        private string ReplaceDynamicValues(string htmlContent, int InvoiceNo)
        {
            var a = context.ImportInvoice.Include(a => a.Supplier)
               .Include(a => a.ProductSize)
               .Include(a => a.ProductSize.Product)
               .Where(a => a.Id == InvoiceNo).FirstOrDefault();
            // Thay thế các giá trị động trong HTML template
            StringBuilder stringData = new StringBuilder(String.Empty);

            stringData.Append($"<tr>");
            stringData.Append($"<td class=\"col-md-9\"> {a.ProductSize.Product.Name} </td>");
            stringData.Append($"<td class=\"col-md-9\"> {a.Quantity} </td>");
            stringData.Append($"<td class=\"col-md-3\"><i class=\"fa fa-inr\"></i> {(a.ImportPrice).ToString("N0") + " VNĐ"} </td>");
            stringData.Append($"</tr>");

            htmlContent = htmlContent.Replace("{{ data.company.name }}", context.Footer.FirstOrDefault().Title.ToString());
            htmlContent = htmlContent.Replace("{{ data.company.phone }}", context.Footer.FirstOrDefault().Phone.ToString());
            htmlContent = htmlContent.Replace("{{ data.company.email }}", context.Footer.FirstOrDefault().Email.ToString());
            htmlContent = htmlContent.Replace("{{ data.company.location }}", context.Footer.FirstOrDefault().Adress.ToString());
            htmlContent = htmlContent.Replace("{{ data.customer.name}}", a.Supplier.Name);
            htmlContent = htmlContent.Replace("{{ data.customer.mobile }}", a.Supplier.Phone);
            htmlContent = htmlContent.Replace("{{ data.customer.email }}", a.Supplier.Email);
            htmlContent = htmlContent.Replace("{{ data.customer.address }}", a.Supplier.Address);
            htmlContent = htmlContent.Replace("{{ data.num_invoice }}", a.Id.ToString());
            htmlContent = htmlContent.Replace("{{ data.total }}", (context.ImportInvoice.Where(a => a.Id == InvoiceNo).FirstOrDefault().ImportPrice * context.ImportInvoice.Where(a => a.Id == InvoiceNo).FirstOrDefault().Quantity).ToString("N0") + " VNĐ");
            htmlContent = htmlContent.Replace("{{ data.date }}", context.ImportInvoice.Where(a => a.Id == InvoiceNo).FirstOrDefault().IssuedDate.ToShortDateString());
            htmlContent = htmlContent.Replace("{{ data }}", stringData.ToString());

            // Thêm các thay thế khác tại đây

            return htmlContent;
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GeneratePDF/{InvoiceNo}")]
        public async Task<IActionResult> GeneratePDF(int InvoiceNo)
        {

            //var htmlContent = System.IO.File.ReadAllText("C:\\Users\\ADMIN\\source\\repos\\BaoDatShop\\BaoDatShop\\wwwroot\\TempletePDFInvoice\\TemplateInovoiceImport.html");
            var htmlContent = System.IO.File.ReadAllText("E:\\BaoDatShop\\BaoDatShop\\wwwroot\\TempletePDFInvoice\\TemplateInovoiceImport.html");
            var replacedHtml = ReplaceDynamicValues(htmlContent, InvoiceNo);

            var document = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = replacedHtml
                    }
                }
            };
            var pdfBytes = _convert.Convert(document);
            return File(pdfBytes, "application/pdf", "output.pdf");
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GetQuanlityProductImport")]
        public async Task<IActionResult> GetQuanlityProductImport()
        {
            return Ok(context.ImportInvoice.Sum(a => a.Quantity));
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpPost("CreateImportInvoice")]
        public async Task<IActionResult> CreateImportInvoice(ImportInvoice model)
        {
            ImportInvoice result = new();
            result.SupplierId = model.SupplierId;
            result.ImportPrice = model.ImportPrice;
            result.Quantity = model.Quantity;

            result.ProductSizeId = model.ProductSizeId;
            result.IssuedDate = DateTime.Now;

            if (IImportInvoiceResponsitories.Create(result) == true)
            {
                var result1=context.ImportInvoice.Include(a=>a.ProductSize).Include(a => a.ProductSize.Product).Where(a=>a.Id==result.Id).FirstOrDefault();
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã lập hóa đơn nhập sản phẩm " + result1.ProductSize.Product.Name;
                IHistoryAccountResponsitories.Create(ab);
                var tam = context.Warehouse.Include(a => a.ProductSize).Where(a => a.ProductSizeId == model.ProductSizeId).FirstOrDefault();
                tam.Stock += model.Quantity;
                context.Update(tam);
                var check = context.SaveChanges();
                if (check > 0)
                    return Ok(true);
                else return Ok(false);
            }
            return Ok(false);
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpPost("UpdateImportInvoice/{id}")]
        public async Task<IActionResult> UpdateImportInvoice(int id, ImportInvoice model)
        {
            ImportInvoice result = IImportInvoiceResponsitories.GetById(id);
            result.SupplierId = model.SupplierId;
            result.ImportPrice = model.ImportPrice;
            result.ProductSizeId = model.ProductSizeId;
            if (IImportInvoiceResponsitories.Update(result) == true)
            {
                return Ok(true);
            }
            return Ok(false);
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GetAllImportInvoice")]
        public async Task<IActionResult> GetAllImportInvoice()
        {
            return Ok(IImportInvoiceResponsitories.GetAll().OrderByDescending(a => a.IssuedDate).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GetAllImportInvoice/{startday},{endday}")]
        public async Task<IActionResult> GetAllImportInvoice(string startday, string endday)
        {
            return Ok(context.ImportInvoice.Include(a => a.Supplier).Include(a => a.ProductSize).Include(a => a.ProductSize.Product).Where(a => a.IssuedDate.Date >= DateTime.Parse(startday)).Where(a => a.IssuedDate.Date <= DateTime.Parse(endday)).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpDelete("DeleteImportInvoice/{id}")]
        public async Task<IActionResult> DeleteImportInvoice(int id)
        {
            var check = context.ImportInvoice.Include(a => a.ProductSize).Include(a => a.ProductSize.Product).Where(a => a.Id == id).FirstOrDefault();
            if (context.Warehouse.Include(a => a.ProductSize).Where(a => a.ProductSizeId == check.ProductSizeId).FirstOrDefault().Stock < check.Quantity)
                return Ok("Thất bại vì sản phẩm đã xuất kho");
            context.Remove(check);
            var a = context.SaveChanges();
            if (a > 0)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã xóa hóa đơn nhập sản phẩm " + check.ProductSize.Product.Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            }
            else
                return Ok("Thất bại");
        }

    }
}
