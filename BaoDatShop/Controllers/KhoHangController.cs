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
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllKhoHang")]
        public async Task<IActionResult> GetAllKhoHang()
        {
            return Ok(IKhoHangResposirity.GetAll());
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpGet("GetSizeOfProduct/{id}")]
        public async Task<IActionResult> GetSizeOfProduct(int id)
        {
            return Ok(IKhoHangResposirity.GetAll().Where(a=>a.ProductSize.ProductId==id).ToList());
        }
    }
}
