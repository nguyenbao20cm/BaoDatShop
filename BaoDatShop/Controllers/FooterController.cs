using BaoDatShop.DTO;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooterController : ControllerBase
    {
        private readonly AppDbContext context;
        public FooterController(AppDbContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateFooter/{id}")]
        public async Task<IActionResult> UpdateBestSellerProduct(int id, FooterRequest model)
        {
            Footer a = context.Footer.Where(a => a.Id == id).FirstOrDefault();
            a.Phone = model.Phone;
            a.Adress = model.Adress;
            a.Email = model.Email;
            a.LinkFacebook = model.LinkFacebook;
            a.LinkInstagram = model.LinkInstagram;
            a.LinkZalo = model.LinkZalo;
            context.Update(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
       
        [HttpGet("GetFooter")]
        public async Task<IActionResult> GetFooter()
        {
            return Ok(context.Footer.FirstOrDefault());
        }
       
     
    }
}
