using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        public ProductsController(IProductService productService, IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.productService = productService;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetQuailityProduct")]
        public async Task<IActionResult> GetQuailityProduct()
        {
            return Ok(productService.GetAll().Count);
        }
        [HttpGet("GetTop10BestSeller")]
        public async Task<IActionResult> GetTop10BestSeller()
        {
            return Ok(productService.GetAll().Where(a => a.ProductType.Status == true).OrderByDescending(a=>a.CountSell).Take(10).ToList().Take(10));
        }
        [HttpGet("GetBestSeller")]
        public async Task<IActionResult> GetBestSeller()
        {
            return Ok(productService.GetAll().Where(a => a.ProductType.Status == true).OrderByDescending(a => a.CountSell).Take(10).ToList());
        }
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(productService.GetAll());
        }
        [HttpGet("GetAllProductStatusTrue")]
        public async Task<IActionResult> GetAllProductStatusTrue()
        {
            return Ok(productService.GetAllProductStatusTrue().Where(a => a.BrandProduct.Status == true).Where(a=>a.ProductType.Status==true));
        }

        [HttpGet("GetAllProductStatusFalse")]
        public async Task<IActionResult> GetAllProductStatusFalse()
        {
            return Ok(productService.GetAllProductStatusFalse());
        }
      
        [HttpGet("GetAllProductInProductType/{id}")]
        public async Task<IActionResult> GetAllProductInProductType(int id)
        {
            return Ok(productService.GetAllProductInProductType(id));
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return Ok( productService.GetById(id));

        }
        [HttpPost("CreateImageProduct")]
        public async Task<IActionResult> CreateImageProduct(IFormFile model)
        {
            var a = productService.CreateImageProduct(model);
            return Ok(productService.CreateImageProduct(model));
        }
        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var a = productService.GetByName(name);
            return Ok(productService.GetByName(name));

        }
        
        //  [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct( CreateProductRequest model)
        {
            
            if (productService.Create(GetCorrectUserId(),model) == true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã tạo sản phẩm "+model.Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            }    
            else
                return Ok("Thất bại");
        }
      //  [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id,  CreateProductRequest model)
        {
            if (productService.Update(id, GetCorrectUserId(),model) == true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã chỉnh sửa sản phẩm " + model.Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            }    
            else
                return Ok("Thất bại");
        }
        //[Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if(productService.Delete(id)==true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã ẩn sản phẩm " + productService.GetById(id).Name;
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
