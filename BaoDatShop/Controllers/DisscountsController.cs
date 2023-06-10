using BaoDatShop.DTO.AdvertisingPanel;
using BaoDatShop.DTO.Disscount;
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
    public class DisscountsController : ControllerBase
    {
        private readonly IDisscountService IDisscountService;
        public DisscountsController(IDisscountService IDisscountService)
        {
            this.IDisscountService = IDisscountService;
        }

        [HttpGet("GetAllDisscountStatusTrue")]
        public async Task<IActionResult> GetAllDisscountStatusTrue()
        {
            return Ok(IDisscountService.GetAllDisscountPanel());
        }
        [HttpGet("GetAllDisscount")]
        public async Task<IActionResult> GetAllDisscount()
        {
            return Ok(IDisscountService.GetAll());
        }
        [HttpGet("GetAllDisscountStatusFalse")]
        public async Task<IActionResult> GetAllDisscountStatusFalse()
        {
            return Ok(IDisscountService.GetAllDisscountStatusFalse());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateDisscount")]
        public async Task<IActionResult> CreateDisscount(CreateDisscount model)
        {
            return Ok(IDisscountService.Create(model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateDisscount/{id}")]
        public async Task<IActionResult> UpdateDisscount(int id, CreateDisscount model)
        {
            return Ok(IDisscountService.Update(id, model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteDisscount/{id}")]
        public async Task<IActionResult> DeleteDisscount(int id)
        {
            return Ok(IDisscountService.Delete(id));
        }
    }
}
