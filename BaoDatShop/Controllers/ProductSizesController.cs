using BaoDatShop.DTO.ProductSize;
using BaoDatShop.DTO.ProductType;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSizesController : ControllerBase
    {
        private readonly IProductSizeService productSizeService;
        public ProductSizesController(IProductSizeService productSizeService)
        {
            this.productSizeService = productSizeService;
        }
        [HttpGet("GetAllProductSizeStatusTrue")]//status true
        public async Task<IActionResult> GetProductType()
        {
            return Ok(productSizeService.GetAllProductTypeStatusTrue());
        }
        [HttpGet("GetAllProductSize")]// all status
        public async Task<IActionResult> GetAllProductType()
        {
            return Ok(productSizeService.GetAll());
        }
        [HttpGet("GetAllProductSizeStatusFalse")]//  status false
        public async Task<IActionResult> GetAllProductTypeStatusFalse()
        {
            return Ok(productSizeService.GetAllProductTypeStatusFalse());
        }

        [HttpGet("GetProductSizeById/{id}")]
        public async Task<IActionResult> GetProductTypeById(int id)
        {
            return Ok(productSizeService.GetById(id));
        }
        //    [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateProductSizeType")]
        public async Task<IActionResult> CreateProductType(CreateProductSize model)
        {
            if (model.Name == string.Empty) return Ok("Không được để trống");
            if (productSizeService.Create(model) == true) return Ok("Thành công");
            else return Ok("Thất bại");

            return Ok(productSizeService.Create(model));
        }
        // [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateProductSize/{id}")]
        public async Task<IActionResult> UpdateProductType(int id, CreateProductSize model)
        {

            if (productSizeService.Update(id, model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        // [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteProductSize/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            if (productSizeService.Delete(id) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
    }
}
