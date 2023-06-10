using BaoDatShop.DTO.AdvertisingPanel;
using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisingPanelsController : ControllerBase
    {
        private readonly IAdvertisingPanelService advertisingPanelService;
        public AdvertisingPanelsController(IAdvertisingPanelService advertisingPanelService)
        {
            this.advertisingPanelService = advertisingPanelService;
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
        public async Task<IActionResult> GetAdvertisingPanelById(string id)
        {
            var a = advertisingPanelService.GetByid(id);
            return Ok(advertisingPanelService.GetByid(id));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateAdvertisingPanel")]
        public async Task<IActionResult> CreateAdvertisingPanel(CreateAdvertisingPanelRequest model)
            {
         
            if (advertisingPanelService.Create(model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
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
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteAdvertisingPanel/{id}")]
        public async Task<IActionResult> DeleteAdvertisingPanel(int id)
        {
            return Ok(advertisingPanelService.Delete(id));
        }
    }
}
