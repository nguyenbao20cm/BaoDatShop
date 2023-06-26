using BaoDatShop.DTO.CreateImageProduct;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoveProductsController : ControllerBase
    {
        private readonly AppDbContext context;
        public LoveProductsController(AppDbContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPut("CreateLoveProducts/{id}")]
        public async Task<IActionResult> CreateLoveProducts(int id) {
            LoveProduct a = new();
            a.AccountId = GetCorrectUserId();
            a.ProductId= id;
            a.Status = true;
            context.Add(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok("Thành công") : Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPut("UpdateLoveProducts/{id}")]
        public async Task<IActionResult> UpdateLoveProducts(int id)
        {
            LoveProduct a = context.LoveProduct.Where(a=>a.Id==id).FirstOrDefault();
            a.Status = false;
            context.Update(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok("Thành công") : Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPut("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(context.LoveProduct.Where(a => a.AccountId == GetCorrectUserId()).ToList());
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}
