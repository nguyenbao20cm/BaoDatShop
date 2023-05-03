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

        [HttpGet("GetAllAdvertisingPanel")]
        public async Task<IActionResult> GetAllAdvertisingPanel()
        {
            return Ok(advertisingPanelService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateAdvertisingPanel")]
        public async Task<IActionResult> CreateAdvertisingPanel(CreateAdvertisingPanelRequest model)
        {
            return Ok(advertisingPanelService.Create(model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateAdvertisingPanel/{id}")]
        public async Task<IActionResult> UpdateAdvertisingPanel(int id, CreateAdvertisingPanelRequest model)
        {
            return Ok(advertisingPanelService.Update(id, model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteAdvertisingPanel/{id}")]
        public async Task<IActionResult> DeleteAdvertisingPanel(int id)
        {
            return Ok(advertisingPanelService.Delete(id));
        }
    }
}
