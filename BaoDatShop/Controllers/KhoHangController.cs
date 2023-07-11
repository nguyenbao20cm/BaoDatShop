using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhoHangController : ControllerBase
    {
        private readonly IKhoHangResposirity IKhoHangResposirity;
        public KhoHangController(IKhoHangResposirity IKhoHangResposirity)
        {
            this.IKhoHangResposirity = IKhoHangResposirity;
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GetAllKhoHang")]
        public async Task<IActionResult> GetAllKhoHang()
        {
            return Ok(IKhoHangResposirity.GetAll());
        }
        [HttpGet("GetSizeOfProduct/{id}")]
        public async Task<IActionResult> GetSizeOfProduct(int id)
        {
            return Ok(IKhoHangResposirity.GetAll().Where(a=>a.ProductSize.ProductId==id).ToList());
        }
    }
}
