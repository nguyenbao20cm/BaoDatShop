using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
               var tam= IProductSizeResponsitories.GetById(model.ProductSizeId);
                tam.Stock += model.Quantity;
                var check =IProductSizeResponsitories.Update(tam);
                if(check==true)
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
            return Ok(IImportInvoiceResponsitories.GetAll().ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpDelete("DeleteImportInvoice/{id}")]
        public async Task<IActionResult> DeleteImportInvoice(int id)
        {
            var check=context.ImportInvoice.Where(a => a.Id == id).FirstOrDefault();
           
            if (context.ProductSize.Where(a => a.Id == check.ProductSizeId).FirstOrDefault().Stock<check.Quantity)
                return Ok(false);
            context.Remove(check);
            var a= context.SaveChanges();
            return a > 0 ? Ok(true) : Ok(false);
        }

    }
}
