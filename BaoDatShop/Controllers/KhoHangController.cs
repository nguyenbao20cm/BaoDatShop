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
        private readonly IWarehouseResposirity IWarehouseResposirity;
        public KhoHangController(IWarehouseResposirity IWarehouseResposirity)
        {
            this.IWarehouseResposirity = IWarehouseResposirity;
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GetAllKhoHang")]
        public async Task<IActionResult> GetAllKhoHang()
        {
            return Ok(IWarehouseResposirity.GetAll());
        }
        [HttpGet("GetStockByProductSizeId/{id}")]
        public async Task<IActionResult> GetStockByProductSizeId(int id)
        {
            return Ok(IWarehouseResposirity.GetAll().Where(a => a.ProductSizeId == id).FirstOrDefault());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GEtSLTonKho")]
        public async Task<IActionResult> GEtSLTonKho()
        {
            return Ok(IWarehouseResposirity.GetAll().Sum(a=>a.Stock));
        }
        [HttpGet("GetSizeOfProduct/{id}")]
        public async Task<IActionResult> GetSizeOfProduct(int id)
        {
            return Ok(IWarehouseResposirity.GetAll().Where(a=>a.ProductSize.Status==true).Where(a=>a.ProductSize.ProductId==id).ToList());
        }
    }
}
