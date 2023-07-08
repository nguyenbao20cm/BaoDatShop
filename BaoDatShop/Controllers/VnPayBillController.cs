using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayBillController : ControllerBase
    {
        private readonly AppDbContext context;
        public VnPayBillController(AppDbContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllVnPayBill")]
        public async Task<IActionResult> GetQuantityAccount()
        {
            return Ok(context.VnpayBill.ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetVNBillFilter/{startday},{endday}")]
        public async Task<IActionResult> GetVNBillFilter(string startday,string endday)
        {
            return Ok(context.VnpayBill.Where(a => DateTime.Parse(a.DateTime) >= DateTime.Parse(startday)).Where(a => DateTime.Parse(a.DateTime) <= DateTime.Parse(endday)).ToList());
        }
    }
}
