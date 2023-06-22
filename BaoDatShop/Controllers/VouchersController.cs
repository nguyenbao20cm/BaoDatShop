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
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("ValidationVoucher")]
        public async Task<IActionResult> ValidationVoucher(string ValidationVoucher)
        {
            return Ok(IVoucherService.ValidationVoucher(ValidationVoucher));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("DeleteVoucher/{id}")]
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
    }
}
