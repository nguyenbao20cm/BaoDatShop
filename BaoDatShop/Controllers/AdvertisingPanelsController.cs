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
        public AdvertisingPanelsController(IAdvertisingPanelService advertisingPanelService, AppDbContext context, IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.advertisingPanelService = advertisingPanelService;
            this.context = context;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
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
        [HttpPost("CreateAdvertisingPanel")]
        public async Task<IActionResult> CreateAdvertisingPanel(CreateAdvertisingPanelRequest model)
        {
            AdvertisingPanel result = new();
            result.Image = model.Image;
            result.ProductId = model.ProductId;
            result.Status = model.Status;
            context.Add(result);
            int check = context.SaveChanges();
            if(check>0)
            {
                HistoryAccount a = new();
                a.AccountID = GetCorrectUserId(); a.Datetime = DateTime.Now;
                a.Content = "Đã thêm 1 pannel quảng cáo";
                IHistoryAccountResponsitories.Create(a);
            }    
            return check > 0 ? Ok(new { data = result, Success = true }) : Ok(" ");
        }
       
        [HttpPost("CreateImageAdvertisingPanel")]
        public async Task<IActionResult> CreateImageAdvertisingPanel(IFormFile model)
        {
            var a = advertisingPanelService.CreateImageAdvertisingPanel(model);
            return Ok(advertisingPanelService.CreateImageAdvertisingPanel(model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateAdvertisingPanel/{id}")]
        public async Task<IActionResult> UpdateAdvertisingPanel(int id, CreateAdvertisingPanelRequest model)
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
        [HttpPut("DeleteAdvertisingPanel/{id}")]
        public async Task<IActionResult> DeleteAdvertisingPanel(int id)
        {
            if(advertisingPanelService.Delete(id)==true)
            {
                HistoryAccount a = new();
                a.AccountID = GetCorrectUserId(); a.Datetime = DateTime.Now;
                a.Content = "Đã xóa pannel quảng cáo";
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
