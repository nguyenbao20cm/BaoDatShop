using BaoDatShop.DTO.Role;
using BaoDatShop.DTO;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandProductsController : ControllerBase
    {
        private readonly AppDbContext context;
       
        public BrandProductsController(AppDbContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateBrandProducts/{id}")]
        public async Task<IActionResult> UpdateFooter(int id,BrandProduct model)
        {
            var a= context.BrandProduct.Where(x => x.Id == id).FirstOrDefault();
            a.Name = model.Name;
            a.Status = model.Status;
            context.Update(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateBrandProducts")]
        public async Task<IActionResult> CreateBrandProducts(BrandProduct model)
        {
            BrandProduct a = new();
            a.Name = model.Name;
            a.Status = model.Status;
            context.Add(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteBrandProducts")]
        public async Task<IActionResult> DeleteBrandProducts(int id )
        {
            BrandProduct a = context.BrandProduct.Where(x => x.Id == id).FirstOrDefault();
            a.Status = false;
            context.Add(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
        [HttpGet("GetAllBrandProducts")]
        public async Task<IActionResult> GetAllBrandProducts()
        {
            return Ok(context.BrandProduct.ToList());
        }
    }
}
