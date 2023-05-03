using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(productService.GetAll());
        }
        [HttpGet("GetAllProductInProductType/{id}")]
        public async Task<IActionResult> GetAllProductInProductType(int id)
        {
            return Ok(productService.GetAllProductInProductType(id));
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return Ok(productService.GetById(id));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm]CreateProductRequest model)
        {
            return Ok(productService.Create(model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProductRequest model)
        {
            return Ok(productService.Update(id, model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return Ok(productService.Delete(id));
        }
    }
}
