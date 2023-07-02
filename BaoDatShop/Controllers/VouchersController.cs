using BaoDatShop.DTO.CreateFavoriteProduct;
using BaoDatShop.DTO.Role;
using BaoDatShop.DTO.Voucher;
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
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherService IVoucherService;
        public VouchersController(IVoucherService IVoucherService)
        {
            this.IVoucherService = IVoucherService;
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllVoucher")]
        public async Task<IActionResult> GetAllVoucher()
        {
            return Ok(IVoucherService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetVoucherByid/{id}")]
        public async Task<IActionResult> GetVoucherByid(int id)
        {
            return Ok(IVoucherService.GetAll().Where(a=>a.Id==id).FirstOrDefault());
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("ValidationVoucher")]
        public async Task<IActionResult> ValidationVoucher(ValidationVoucher ValidationVoucher)
        {
            if (IVoucherService.ValidationVoucher(ValidationVoucher.Name) == null) return Ok("null");
            return Ok(IVoucherService.ValidationVoucher(ValidationVoucher.Name));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteVoucher/{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            return Ok(IVoucherService.Delete(id));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateVoucher")]
        public async Task<IActionResult> CreateVoucher(CreateVoucher model)
        {
            if (IVoucherService.Create(model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateVoucher/{id}")]
        public async Task<IActionResult> UpdateVoucher(int id,CreateVoucher model)
        {
            if (IVoucherService.Update(id,model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
    }
}
