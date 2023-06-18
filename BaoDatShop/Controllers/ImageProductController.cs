using BaoDatShop.DTO.CreateImageProduct;
using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Product;
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
    public class ImageProductController : ControllerBase
    {
        private readonly IImageProductService IImageProductService;
        public ImageProductController(IImageProductService IImageProductService)
        {
            this.IImageProductService = IImageProductService;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateImageProduct/{id}")]
        public async Task<IActionResult> UpdateImageProduct(int id, CreateImageProduct model)
        {
            if (IImageProductService.Update(id, model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateImagesProduct")]
        public async Task<IActionResult> CreateImagesProduct(CreateImageProduct model)
        {
            return Ok(IImageProductService.Create(model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllImageProduct")]
        public async Task<IActionResult> GetAllImageProduct()
        {
            return Ok(IImageProductService.GetAll());
        }
        [HttpGet("GetAllImageProductStatusTrue")]
        public async Task<IActionResult> GetAllImageProductStatusTrue()
        {
            return Ok(IImageProductService.GetAllImageProductStatusTrue());
        }

        [HttpGet("GetAllImageProductStatusFalse")]
        public async Task<IActionResult> GetAllImageProductStatusFalse()
        {
            return Ok(IImageProductService.GetAllImageProductStatusFalse());
        }
        [HttpGet("GetImageProductById/{id}")]
        public async Task<IActionResult> GetImageProductById(int id)
        {
            return Ok(IImageProductService.GetById(id));

        }
        [HttpGet("GetAllImageProductById/{id}")]
        public async Task<IActionResult> GetAllImageProductById(int id)
        {
            return Ok(IImageProductService.GetAllImageProductById(id));

        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }

    }
}
