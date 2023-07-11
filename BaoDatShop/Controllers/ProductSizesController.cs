using BaoDatShop.DTO.ProductSize;
using BaoDatShop.DTO.ProductType;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSizesController : ControllerBase
    {
        private readonly IImportInvoiceResponsitories IImportInvoiceResponsitories;
        private readonly IProductSizeService productSizeService;
        private readonly IProductService IProductService;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        private readonly AppDbContext context;
        public ProductSizesController(IHistoryAccountResponsitories IHistoryAccountResponsitories,
            IProductSizeService productSizeService,
            AppDbContext context,
            IImportInvoiceResponsitories IImportInvoiceResponsitories,
            IProductService IProductService)
        {
            this.IImportInvoiceResponsitories = IImportInvoiceResponsitories;
            this.productSizeService = productSizeService;
            this.IProductService = IProductService;
            this.context = context;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
        }
 
        [HttpGet("GetAllProductSizeStatusTrue")]//status true
        public async Task<IActionResult> GetProductType()
        {
            return Ok(productSizeService.GetAllProductTypeStatusTrue());
        }

        [HttpGet("GetProductSizeByProductId/{Id}")]
        public async Task<IActionResult> GetProductSizeByProductId(int Id)
        {
            return Ok(productSizeService.GetProductSizeById(Id));
        }

        [HttpGet("GetByid/{Id}")]
        public async Task<IActionResult> GetByid(int Id)
        {
            return Ok(productSizeService.GetById(Id));
        }
        [HttpGet("GetProductIdByProductSize/{Id}")]
        public async Task<IActionResult> GetProductIdByProductSize(int Id)
        {
            return Ok(productSizeService.GetProductIdByProductSize(Id));
        }
     
        [HttpGet("GetAllProductSize")]// all status
        public async Task<IActionResult> GetAllProductSize()
        {
            return Ok(productSizeService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GetAllProductSizeStatusFalse")]//  status false
        public async Task<IActionResult> GetAllProductTypeStatusFalse()
        {
            return Ok(productSizeService.GetAllProductTypeStatusFalse());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GetAllImportPrice/{year}")]//  status false
        public async Task<IActionResult> GetAllImportPrice(string year)
        {
            return Ok(productSizeService.GetAllImportPrice(year));
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GetImportDayAllYear/{year}")]//  status false
        public async Task<IActionResult> GetImportDayAllYear(int year)
        {
            var a = IImportInvoiceResponsitories.GetAll().Where(a=>a.IssuedDate.Year==year);
            var tong = 0;
            foreach(var item in a)
            {
                tong += item.ImportPrice*item.Quantity;
            }    
            return Ok(tong);
        }

        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpPost("CreateProductSize")]
        public async Task<IActionResult> CreateProductSize(CreateProductSize model)
        {
           
            var b = productSizeService.GetAll().Where(a=>a.ProductId==model.ProductId).ToList();
            foreach(var item in b)
            {
                if (item.Name == model.Name)
                {
                    return Ok("Size da ton tai");
                }    
            }    
            if (model.Name == string.Empty) return Ok("Không được để trống");
            if (productSizeService.Create(GetCorrectUserId(),model) == true)
            {
                return Ok("Thành công");
            }    
            else return Ok("Thất bại");
            return Ok(productSizeService.Create(GetCorrectUserId(),model));
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpPut("UpdateProductSize/{id}")]
        public async Task<IActionResult> UpdateProductType(int id, UpdateProductSize model)
        {
            
              
            if (model.Status==true)
            {
                var b = productSizeService.GetAll().Where(a => a.ProductId == model.ProductId).ToList();
                foreach (var item in b)
                {
                    if (item.Name == model.Name)
                        if (item.Status == true)
                        return Ok("Không được vì sản phẩm đã có Size này hiện thị");
                }
            }    
            if (productSizeService.Update(id, GetCorrectUserId(), model) == true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã chỉnh sửa size sản phẩm " + productSizeService.GetById(id).Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            }    
               
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpPut("DeleteProductSize/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            if (productSizeService.Delete(id) == true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã ẩn size sản phẩm " + productSizeService.GetById(id).Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            } 
            else
                return Ok("Thất bại");
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}
