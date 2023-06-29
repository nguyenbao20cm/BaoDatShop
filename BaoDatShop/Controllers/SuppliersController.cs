using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Mail;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext context;
        public SuppliersController(AppDbContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllSupplier")]
        public async Task<IActionResult> GetAllSupplier()
        {
            return Ok(context.Supplier.ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateSupplier")]
        public async Task<IActionResult> CreateSupplier(Supplier model)
        {
                Supplier a = new();
                a.Name= model.Name; a.Phone = model.Phone;
            a.Email = model.Email;
                a.Address = model.Address;
            a.TaxCode = model.TaxCode;
            context.Add(a);
                int check = context.SaveChanges();
                return check > 0 ? Ok(true) : Ok(false);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateSupplier/{id}")]
        public async Task<IActionResult> UpdateSupplier(int id,Supplier model)
        {
            Supplier a = context.Supplier.Where(a=>a.Id==id).FirstOrDefault();
            a.Name = model.Name;
            a.Phone = model.Phone;
            a.Email = model.Email; a.TaxCode = model.TaxCode;
            a.Address = model.Address;
            context.Update(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
    }
}
