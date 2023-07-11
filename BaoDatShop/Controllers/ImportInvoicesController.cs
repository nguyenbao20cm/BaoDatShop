using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportInvoicesController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IImportInvoiceResponsitories IImportInvoiceResponsitories;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        private readonly IProductSizeResponsitories IProductSizeResponsitories;
        public ImportInvoicesController(IHistoryAccountResponsitories IHistoryAccountResponsitories, IProductSizeResponsitories IProductSizeResponsitories,
            IImportInvoiceResponsitories IImportInvoiceResponsitories,AppDbContext context)
        {
            this.context=context;
            this.IProductSizeResponsitories = IProductSizeResponsitories;
            this.IImportInvoiceResponsitories = IImportInvoiceResponsitories;
            this.IHistoryAccountResponsitories =IHistoryAccountResponsitories;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateImportInvoice")]
        public async Task<IActionResult> CreateImportInvoice(ImportInvoice model)
        {
           ImportInvoice result = new();
            result.SupplierId = model.SupplierId;
            result.ImportPrice = model.ImportPrice;
            result.Quantity=model.Quantity;
          
            result.ProductSizeId = model.ProductSizeId;
            result.IssuedDate = DateTime.Now;
           
            if ( IImportInvoiceResponsitories.Create(result)==true)
            {
               var tam= context.KHoHang.Include(a=>a.ProductSize).Where(a=>a.ProductSizeId==model.ProductSizeId).FirstOrDefault();
                tam.Stock += model.Quantity;
                context.Update(tam);
                var check =context.SaveChanges();
                if(check>0)
                    return Ok(true);
                else return Ok(false);
            }
            return Ok(false);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("UpdateImportInvoice/{id}")]
        public async Task<IActionResult> UpdateImportInvoice(int id,ImportInvoice model)
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
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllImportInvoice")]
        public async Task<IActionResult> GetAllImportInvoice()
        {
            return Ok(IImportInvoiceResponsitories.GetAll().OrderByDescending(a => a.IssuedDate).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllImportInvoice/{startday},{endday}")]
        public async Task<IActionResult> GetAllImportInvoice(string startday,string endday)
        {
            return Ok(context.ImportInvoice.Include(a => a.Supplier).Include(a => a.ProductSize).Include(a => a.ProductSize.Product).Where(a=>a.IssuedDate.Date>=DateTime.Parse(startday)).Where(a=>a.IssuedDate.Date<=DateTime.Parse(endday)).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpDelete("DeleteImportInvoice/{id}")]
        public async Task<IActionResult> DeleteImportInvoice(int id)
        {
            var check=context.ImportInvoice.Where(a => a.Id == id).FirstOrDefault();
            if (context.KHoHang.Include(a=>a.ProductSize).Where(a => a.ProductSizeId == check.ProductSizeId).FirstOrDefault().Stock<check.Quantity)
                return Ok("Thất bại vì sản phẩm đã xuất kho");
            context.Remove(check);
            var a= context.SaveChanges();
            return a > 0 ? Ok("Thành công") : Ok("Thất bại");
        }

    }
}
