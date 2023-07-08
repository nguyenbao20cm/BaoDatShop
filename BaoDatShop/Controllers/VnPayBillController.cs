using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
