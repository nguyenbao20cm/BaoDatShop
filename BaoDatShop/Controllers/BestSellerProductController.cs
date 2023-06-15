using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestSellerProductController : ControllerBase
    {
        private readonly AppDbContext context;
        public BestSellerProductController(AppDbContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateBestSellerProduct")]
        public async Task<IActionResult> CreateBestSellerProduct(BestSellerRequest model)
        {
            BestSellerProduct result = new();
            result.ProductId = model.ProductId;
            result.Status = model.Status;
            context.Add(result);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteBestSellerProduct/{id}")]
        public async Task<IActionResult> DeleteBestSellerProduct(int id)
        {
            BestSellerProduct a = context.BestSellerProduct.Where(a => a.Id == id).FirstOrDefault();
            context.Remove(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateBestSellerProduct/{id}")]
        public async Task<IActionResult> UpdateBestSellerProduct(int id, BestSellerRequest model)
        {
            BestSellerProduct a = context.BestSellerProduct.Where(a => a.Id == id).FirstOrDefault();
            a.Status = model.Status;
            a.ProductId = model.ProductId;
            context.Update(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllBestSellerProduct")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(context.BestSellerProduct.ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllBestSellerProductStatusTrue")]
        public async Task<IActionResult> GetAllStatusTrue()
        {
            return Ok(context.BestSellerProduct.Where(a => a.Status == true).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllBestSellerProductStatusFalse")]
        public async Task<IActionResult> GetAllStatusFalse()
        {
            return Ok(context.BestSellerProduct.Where(a => a.Status == false).ToList());
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}
