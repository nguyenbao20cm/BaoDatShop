using BaoDatShop.DTO.AdvertisingPanel;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
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
    public class AdvertisingPanelsController : ControllerBase
    {
        private readonly IAdvertisingPanelService advertisingPanelService; 
        private readonly AppDbContext context;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        private readonly IWebHostEnvironment _environment;
        public AdvertisingPanelsController(IWebHostEnvironment _environment,IAdvertisingPanelService advertisingPanelService, AppDbContext context, IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.advertisingPanelService = advertisingPanelService;
            this.context = context;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
            this._environment = _environment;
        }

        [HttpGet("GetAllAdvertisingPanelStatusTrue")]
        public async Task<IActionResult> GetAllAdvertisingPane1l()
        {
            return Ok(advertisingPanelService.GetAll());
        }
        [HttpGet("GetAllAdvertisingPanel")]
        public async Task<IActionResult> GetAllAdvertisingPanel()
        {
            return Ok(advertisingPanelService.GetAllAdvertisingPanel());
        }
        [HttpGet("GetAllAdvertisingPanelStatusFalse")]
        public async Task<IActionResult> GetAllAdvertisingPanelStatusFalse()
        {
            return Ok(advertisingPanelService.GetAllAdvertisingPanelStatusFalse());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAdvertisingPanelById/{id}")]
        public async Task<IActionResult> GetAdvertisingPanelById(int id)
        {
            var a = advertisingPanelService.GetByid(id);
            return Ok(advertisingPanelService.GetByid(id));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateAdvertisingPanel/{ProductId},{Title},{Content}")]
        public async Task<IActionResult> CreateAdvertisingPanel(int ProductId,string Title, string Content, IFormFile model)
        {
            AdvertisingPanel result = new();
            result.Image = "";
            result.ProductId = ProductId;
            result.Status = true;
            result.Title = Title;
            result.Content = Content;
            context.Add(result);
            int check = context.SaveChanges();
            if(check>0)
            {
                result.Image = result.AdvertisingPanelID+".jpg";
                context.Update(result);
                var check1=context.SaveChanges();
                if(check1>0)
                {
                    var fileName = result.AdvertisingPanelID + ".jpg";
                    var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "AdvertisingPanel");
                    var uploadPath = Path.Combine(uploadFolder, fileName);
                    using (FileStream fs = System.IO.File.Create(uploadPath))
                    {
                        model.CopyTo(fs);
                        fs.Flush();
                    }
                    HistoryAccount a = new();
                    a.AccountID = GetCorrectUserId(); a.Datetime = DateTime.Now;
                    a.Content = "Đã thêm 1 pannel quảng cáo";
                    IHistoryAccountResponsitories.Create(a);
                    return Ok(true);
                }    
            }
            return Ok(false);
            //return check > 0 ? Ok(new { data = result, Success = true }) : Ok(" ");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateAdvertisingPanel/{ProductId},{Title},{Content}")]
        public async Task<IActionResult> UpdateAdvertisingPanel(int ProductId, string Title, string Content, IFormFile model)
        {
            AdvertisingPanel result = new();
            result.ProductId = ProductId;
            result.Title = Title;
            result.Content = Content;
            context.Update(result);
            int check = context.SaveChanges();
            if (check > 0)
            {
                    var fileName = result.AdvertisingPanelID + ".jpg";
                    var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "AdvertisingPanel");
                    var uploadPath = Path.Combine(uploadFolder, fileName);
                    using (FileStream fs = System.IO.File.Create(uploadPath))
                    {
                        model.CopyTo(fs);
                        fs.Flush();
                    }
                    HistoryAccount a = new();
                    a.AccountID = GetCorrectUserId(); a.Datetime = DateTime.Now;
                    a.Content = "Đã sửa 1 pannel quảng cáo";
                    IHistoryAccountResponsitories.Create(a);
                    return Ok(true);
            
            }
            return Ok(false);
            //return check > 0 ? Ok(new { data = result, Success = true }) : Ok(" ");
        }
        [HttpPost("CreateImageAdvertisingPanel")]
        public async Task<IActionResult> CreateImageAdvertisingPanel(IFormFile model)
        {
            var a = advertisingPanelService.CreateImageAdvertisingPanel(model);
            return Ok(advertisingPanelService.CreateImageAdvertisingPanel(model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateAdvertisingPanel/{id}")]
        public async Task<IActionResult> UpdateAdvertisingPanel(int id, AdvertisingPanel model)
        {
          
            if (advertisingPanelService.Update(id, model) == true)
            {
                    HistoryAccount a = new();
                    a.AccountID = GetCorrectUserId(); a.Datetime = DateTime.Now;
                    a.Content = "Đã chỉnh sửa pannel quảng cáo";
                    IHistoryAccountResponsitories.Create(a);
                return Ok("Thành công");
            }    
                
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpDelete("DeleteAdvertisingPanel/{id}")]
        public async Task<IActionResult> DeleteAdvertisingPanel(int id)
        {
            if(advertisingPanelService.Delete(id)==true)
            {
                HistoryAccount a = new();
                a.AccountID = GetCorrectUserId(); a.Datetime = DateTime.Now;
                a.Content = "Đã xóa 1 pannel quảng cáo";
                IHistoryAccountResponsitories.Create(a);
                return Ok("Thành công");
            }    
            return Ok("Thất bại");
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}
