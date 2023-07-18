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
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
            this.context = context;
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpGet("GetAllSupplier")]
        public async Task<IActionResult> GetAllSupplier()
        {
            return Ok(context.Supplier.ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpPost("CreateSupplier")]
        public async Task<IActionResult> CreateSupplier(Supplier model)
        {
            if(context.Supplier.Where(a => a.TaxCode == model.TaxCode).FirstOrDefault()!=null) return Ok("Mã thuế đã tồn tại");
            if (context.Supplier.Where(a => a.Phone == model.Phone).FirstOrDefault() != null) return Ok("Số điện thoại nhà cung cấp đã tồn tại");
            if (context.Supplier.Where(a => a.Email == model.Email).FirstOrDefault() != null) return Ok("Emal nhà cung cấp đã tồn tại");
            if (context.Supplier.Where(a => a.Name == model.Name).FirstOrDefault() != null) return Ok("Tên nhà cung cấp đã tồn tại");
            Supplier a = new();
            a.Name = model.Name; a.Phone = model.Phone;
            a.Email = model.Email;
            a.Address = model.Address;
            a.TaxCode = model.TaxCode;
            context.Add(a);
            int check = context.SaveChanges();
            if (check > 0)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã tạo nhà cung cấp " + model.Name;
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
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO)]
        [HttpPut("UpdateSupplier/{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, Supplier model)
        {   
            if(context.Supplier.Where(a => a.Id == id).FirstOrDefault().TaxCode !=model.TaxCode)
            {
                if (context.Supplier.Where(a => a.TaxCode == model.TaxCode).FirstOrDefault() != null) return Ok("Mã thuế đã tồn tại");
            }
            if (context.Supplier.Where(a => a.Id == id).FirstOrDefault().Phone != model.Phone)
            {
                if (context.Supplier.Where(a => a.Phone == model.Phone).FirstOrDefault() != null) return Ok("Số điện thoại nhà cung cấp đã tồn tại");
            }
            if (context.Supplier.Where(a => a.Id == id).FirstOrDefault().Email != model.Email)
            {
                if (context.Supplier.Where(a => a.Email == model.Email).FirstOrDefault() != null) return Ok("Emal nhà cung cấp đã tồn tại");
            }
            if (context.Supplier.Where(a => a.Id == id).FirstOrDefault().Name != model.Name)
            {
                if (context.Supplier.Where(a => a.Name == model.Name).FirstOrDefault() != null) return Ok("Tên nhà cung cấp đã tồn tại");
            }
            Supplier a = context.Supplier.Where(a => a.Id == id).FirstOrDefault();
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
