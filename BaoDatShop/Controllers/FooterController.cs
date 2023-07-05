using BaoDatShop.DTO;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooterController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _environment;
        public FooterController(AppDbContext context, IWebHostEnvironment _environment)
        {
            this.context = context;
            this._environment = _environment;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateFooter/{id}")]
        public async Task<IActionResult> UpdateFooter(int id, Footer model)
        {
            Footer a = context.Footer.Where(a => a.Id == id).FirstOrDefault();
            a.Avatar = model.Avatar;
            a.Title = model.Title;
            a.Phone = model.Phone;
            a.Adress = model.Adress;
            a.Email = model.Email;
            a.LinkFacebook = model.LinkFacebook;
            a.LinkInstagram = model.LinkInstagram;
            a.LinkZalo = model.LinkZalo;
            a.MaQR = model.MaQR;
            a.SoBank = model.SoBank;
            a.Bank = model.Bank;
            a.TenNguoiBank = model.TenNguoiBank;
            context.Update(a);
            int check = context.SaveChanges();
            return check > 0 ? Ok(true) : Ok(false);
        }
        [HttpPost("CreateImageQRFooter")]
        public async Task<IActionResult> CreateImageQRFooter(IFormFile model)
        {
            try
            {
                var fileName = "1.jpg";
                var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "ImageQr");
                var uploadPath = Path.Combine(uploadFolder, fileName);
                using (FileStream fs = System.IO.File.Create(uploadPath))
                {
                    model.CopyTo(fs);
                    fs.Flush();
                }
                return Ok(true);
            }
            catch (Exception e)
            {
                return Ok(false);
            }

        }
        [HttpPost("CreateImageFooter")]
        public async Task<IActionResult> CreateImageFooter(IFormFile model)
        {
            try {
                var fileName = "1.jpg";
                var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "AvatarWebsite");
                var uploadPath = Path.Combine(uploadFolder, fileName);
                using (FileStream fs = System.IO.File.Create(uploadPath))
                {
                    model.CopyTo(fs);
                    fs.Flush();
                }
                return Ok(true);
            }
            catch (Exception e)
            {
                return Ok(false);
            }
           
        }
        [HttpGet("GetFooter")]
        public async Task<IActionResult> GetFooter()
        {
            return Ok(context.Footer.FirstOrDefault());
        }
    }
}
