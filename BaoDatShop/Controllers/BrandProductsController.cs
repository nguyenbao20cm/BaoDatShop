using BaoDatShop.DTO.Role;
using BaoDatShop.DTO;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using BaoDatShop.Responsitories;
using System.Security.Claims;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandProductsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        public BrandProductsController(AppDbContext context,IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.context = context;
           this. IHistoryAccountResponsitories= IHistoryAccountResponsitories;
        }
        [HttpGet("GetProductByBrandProductsId/{id}")]
        public async Task<IActionResult> GetProductByBrandProductsId(int id)
        {
            return Ok(context.Product.Where(a=>a.BrandProductId==id).ToList());
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
      
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpPut("UpdateBrandProducts/{id}")]
        public async Task<IActionResult> UpdateFooter(int id,BrandProduct model)
        {
            var a= context.BrandProduct.Where(x => x.Id == id).FirstOrDefault();
            a.Name = model.Name;
            a.Status = model.Status;
            context.Update(a);
            int check = context.SaveChanges();
            if(check>0)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã chỉnh sửa thương hiệu có id là "+id;
                IHistoryAccountResponsitories.Create(ab);
            }    
            return check > 0 ? Ok(true) : Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpPost("CreateBrandProducts")]
        public async Task<IActionResult> CreateBrandProducts(BrandProduct model)
        {
            BrandProduct a = new();
            a.Name = model.Name;
            a.Status = model.Status;
            context.Add(a);
            int check = context.SaveChanges();
            if (check > 0)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã tạo thương hiệu " + model.Name;
                IHistoryAccountResponsitories.Create(ab);
            }
            return check > 0 ? Ok(true) : Ok(false);
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpPut("DeleteBrandProducts")]
        public async Task<IActionResult> DeleteBrandProducts(int id )
        {
            BrandProduct a = context.BrandProduct.Where(x => x.Id == id).FirstOrDefault();
            a.Status = false;
            context.Add(a);
            int check = context.SaveChanges();
            if (check > 0)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã xóa thương hiệu " + context.BrandProduct.Where(x => x.Id == id).FirstOrDefault().Name;
                IHistoryAccountResponsitories.Create(ab);
            }
            return check > 0 ? Ok(true) : Ok(false);
        }
        [HttpGet("GetAllBrandProducts")]
        public async Task<IActionResult> GetAllBrandProducts()
        {
            return Ok(context.BrandProduct.ToList());
        }
    }
}
