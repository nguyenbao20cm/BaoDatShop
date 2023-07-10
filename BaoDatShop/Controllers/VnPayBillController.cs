using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Globalization;

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
            var result = context.VnpayBill
                .Where(a => a.DateTime.Date >= DateTime.Parse(startday).Date)
                .Where(a => a.DateTime.Month >= DateTime.Parse(startday).Month)
                .Where(a => a.DateTime.Year >= DateTime.Parse(startday).Year)
                .Where(a => a.DateTime.Date <= DateTime.Parse(endday).Date)
                     .Where(a => a.DateTime.Month <= DateTime.Parse(endday).Month)
                          .Where(a => a.DateTime.Year <= DateTime.Parse(endday).Year)
                .ToList();
            return Ok(result);
        }
       
    }
}
