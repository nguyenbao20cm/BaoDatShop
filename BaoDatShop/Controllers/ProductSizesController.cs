using BaoDatShop.DTO.ProductSize;
using BaoDatShop.DTO.ProductType;
using BaoDatShop.DTO.Role;
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
        private readonly IProductSizeService productSizeService;
        private readonly IProductService IProductService;
        public ProductSizesController(IProductSizeService productSizeService, IProductService IProductService)
        {
            this.productSizeService = productSizeService;
            this.IProductService = IProductService;
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
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllProductSizeStatusFalse")]//  status false
        public async Task<IActionResult> GetAllProductTypeStatusFalse()
        {
            return Ok(productSizeService.GetAllProductTypeStatusFalse());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllImportPrice/{year}")]//  status false
        public async Task<IActionResult> GetAllImportPrice(string year)
        {
            return Ok(productSizeService.GetAllImportPrice(year));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetImportDayAllYear/{year}")]//  status false
        public async Task<IActionResult> GetImportDayAllYear(int year)
        {
            var a = productSizeService.GetAll().Where(a=>a.IssuedDate.Year==year);
            var tong = 0;
            foreach(var item in a)
            {
                tong += item.ImportPrice;
            }    
            return Ok(tong);
        }
        //[Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllImportPriceByDay")]
        public async Task<IActionResult> GetAllImportPriceByDay()
        {
            var query =  productSizeService.GetAll()
              .GroupBy(hd => hd.IssuedDate.Date)
              .Select(g => new
              {
                  NgayLap = g.Key,
                  TongTien = g.Sum(hd => hd.ImportPrice*hd.Stock),
              })
              .ToList();
            return Ok(query);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateProductSize")]
        public async Task<IActionResult> CreateProductSize(CreateProductSize model)
        {
            if(IProductService.GetById(model.ProductId).Price<model.ImportPrice)
                return Ok("Giá nhập không thể lớn hơn giá bán");
            var a = 0;
            var b = productSizeService.GetAll().Where(a=>a.ProductId==model.ProductId).ToList();
            foreach(var item in b)
            {
                if (item.Name == model.Name)
                {
                    model.Status = false;
                    if (productSizeService.Create(GetCorrectUserId(), model) == true) return Ok("Thành công, nhưng trạng thái thành Ẩn vì Size này chỉ được 1 hiện thị ");
                    else return Ok("Thất bại");
                }    
            }    
            if (model.Name == string.Empty) return Ok("Không được để trống");
            if (productSizeService.Create(GetCorrectUserId(),model) == true) return Ok("Thành công");
            else return Ok("Thất bại");

            return Ok(productSizeService.Create(GetCorrectUserId(),model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateProductSize/{id}")]
        public async Task<IActionResult> UpdateProductType(int id, UpdateProductSize model)
        {
            if (IProductService.GetById(model.ProductId).Price < model.ImportPrice)
                return Ok("Giá nhập không thể nhỏ hơn giá bán");
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
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteProductSize/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            if (productSizeService.Delete(id) == true)
                return Ok("Thành công");
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
