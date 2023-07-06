using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Mail;
using System.Security.Claims;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        private readonly AppDbContext context;
        public SuppliersController(AppDbContext context, IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.IHistoryAccountResponsitories =IHistoryAccountResponsitories;
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
            if(check>0)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã tạo nhà cung cấp "+model.Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok(true);
            }    
                return check > 0 ? Ok(true) : Ok(false);
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
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
            if (check > 0)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã chỉnh sửa nhà cung cấp " + model.Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok(true);
            }
            return check > 0 ? Ok(true) : Ok(false);
        }
    }
}
