
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using BaoDatShop.DTO.Product;
using BaoDatShop.Model.Model;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialProductsController : ControllerBase
    {
        private readonly AppDbContext context;
        public SpecialProductsController(AppDbContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateSpecialProducts")]
        public async Task<IActionResult> CreateSpecialProducts(BestSellerRequest model)
        {
            SpecialProduct result = new();
            result.ProductId = model.ProductId;
            result.Status = model.Status;
            context.Add(result);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteSpecialProducts/{id}")]
        public async Task<IActionResult> DeleteSpecialProducts(int id)
        {
            SpecialProduct a = context.SpecialProduct.Where(a => a.Id == id).FirstOrDefault();
            context.Remove(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateSpecialProducts/{id}")]
        public async Task<IActionResult> UpdateSpecialProducts(int id, BestSellerRequest model)
        {
            SpecialProduct a = context.SpecialProduct.Where(a => a.Id == id).FirstOrDefault();
            a.Status = model.Status;
            a.ProductId = model.ProductId;
            context.Update(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllSpecialProducts")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(context.SpecialProduct.ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllSpecialProductsStatusTrue")]
        public async Task<IActionResult> GetAllStatusTrue()
        {
            return Ok(context.SpecialProduct.Where(a=>a.Status==true).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllSpecialProductsStatusFalse")]
        public async Task<IActionResult> GetAllStatusFalse()
        {
            return Ok(context.SpecialProduct.Where(a => a.Status == false).ToList());
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}
