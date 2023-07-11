using BaoDatShop.DTO.ProductType;
using BaoDatShop.DTO.Role;
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
    public class ProductTypesController : ControllerBase
    {
        private readonly IProductTypeService productypeService;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        public ProductTypesController(IProductTypeService productypeService, IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.productypeService = productypeService;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
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
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpPost("CreateProductType")]
        public async Task<IActionResult> CreateProductType(CreateProductTypeRequest model)
        {
            if (model.Name == string.Empty) return Ok("Không được để trống");
            if (productypeService.Create(model) == true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã tạo loại sản phẩm " + model.Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            } 
            else return Ok("Thất bại");
            
            return Ok(productypeService.Create(model));
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpPut("UpdateProductType/{id}")]
        public async Task<IActionResult> UpdateProductType(int id, CreateProductTypeRequest model)
        {
         
            if (productypeService.Update(id, model) == true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã chỉnh sửa loại sản phẩm " + model.Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            }  
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
