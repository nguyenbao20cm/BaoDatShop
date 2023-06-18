using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.Role;
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
        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }
     
        [HttpGet("GetTop10BestSeller")]
        public async Task<IActionResult> GetTop10BestSeller()
        {
            return Ok(productService.GetAll().OrderByDescending(a=>a.CountSell).Take(10).ToList());
        }
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(productService.GetAll());
        }
        [HttpGet("GetAllProductStatusTrue")]
        public async Task<IActionResult> GetAllProductStatusTrue()
        {
            return Ok(productService.GetAllProductStatusTrue());
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
            
            if (productService.Create(model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
      //  [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id,  CreateProductRequest model)
        {
            if (productService.Update(id, model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        //[Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if(productService.Delete(id)==true)
            return Ok("Thành công");
            else
                return Ok("Thất bại");

        }
    }
}
