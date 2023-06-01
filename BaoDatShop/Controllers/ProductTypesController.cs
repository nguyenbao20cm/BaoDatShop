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
        [HttpGet("GetAllProductTypeStatusTrue")]//status true
        public async Task<IActionResult> GetProductType()
        {
            return Ok(productypeService.GetAllProductTypeStatusTrue());
        }
        [HttpGet("GetAllProductType")]// all status
        public async Task<IActionResult> GetAllProductType()
        {
            return Ok(productypeService.GetAll());
        }
        [HttpGet("GetAllProductTypeStatusFalse")]//  status false
        public async Task<IActionResult> GetAllProductTypeStatusFalse()
        {
            return Ok(productypeService.GetAllProductTypeStatusFalse());
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
            if (model.Name == string.Empty) return Ok("Không được để trống");
            if (productypeService.Create(model) == true) return Ok("Thành công");
            else return Ok("Thất bại");
            
            return Ok(productypeService.Create(model));
        }
       // [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateProductType/{id}")]
        public async Task<IActionResult> UpdateProductType(int id, CreateProductTypeRequest model)
        {
         
            if (productypeService.Update(id, model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
       // [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteProductType/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            if(productypeService.Delete(id)==true)
            return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
    }
}
