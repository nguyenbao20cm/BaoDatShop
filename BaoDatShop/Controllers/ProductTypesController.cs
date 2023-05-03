using BaoDatShop.DTO.ProductType;
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
    public class ProductTypesController : ControllerBase
    {
        private readonly IProductTypeService productypeService;
        public ProductTypesController(IProductTypeService productypeService)
        {
            this.productypeService = productypeService;
        }
        [HttpGet("GetAllProductType")]
        public async Task<IActionResult> GetAllProductType()
        {
            return Ok(productypeService.GetAll());
        }
        [HttpGet("GetProductTypeById/{id}")]
        public async Task<IActionResult> GetProductTypeById(int id)
        {
            return Ok(productypeService.GetById(id));
        }
    //    [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateProductType")]
        public async Task<IActionResult> CreateProductType(CreateProductTypeRequest model)
        {
            return Ok(productypeService.Create(model));
        }
       // [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateProductType/{id}")]
        public async Task<IActionResult> UpdateProductType(int id, CreateProductTypeRequest model)
        {
            return Ok(productypeService.Update(id, model));
        }
       // [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteProductType/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            return Ok(productypeService.Delete(id));
        }
    }
}
