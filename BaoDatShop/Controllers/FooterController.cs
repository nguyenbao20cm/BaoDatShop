using BaoDatShop.DTO;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooterController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _environment;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        public FooterController(AppDbContext context, IWebHostEnvironment _environment, IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.context = context;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
            this._environment = _environment;
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
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
            context.Update(a);
            int check = context.SaveChanges();
            if (check > 0)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã chỉnh sửa thông tin trang web";
                IHistoryAccountResponsitories.Create(ab);
            }
            return check > 0 ? Ok(true) : Ok(false);
        }
       
        [HttpPost("CreateImageFooter")]
        public async Task<IActionResult> CreateImageFooter(IFormFile model)
        {
            try {
                var fileName = "1.jpg";
                var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Footer");
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
            if(context.Footer==null) return Ok(null);
            if (context.Footer.FirstOrDefault() != null)
                return Ok(context.Footer.FirstOrDefault());
            else
                return Ok(null);
        }
    }
}
